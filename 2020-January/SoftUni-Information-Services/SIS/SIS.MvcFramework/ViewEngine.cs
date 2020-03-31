namespace SIS.MvcFramework
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class ViewEngine : IViewEngine
    {
        private const string AppViewAssemblyName = "AppViewAssembly";
        private const string AppViewNamespace = "AppViewNamespace";
        private const string AppViewCode = "AppViewCode";
        
        /// <summary>
        /// Returns final HTML from a template after parsing C# code.
        /// </summary>
        /// <param name="templateHtml">An html template that can have Razor-like structure with C# Code</param>
        /// <param name="model">Model is of type object since when it's being passed T when called. When Type is being passed to object, the object knows exactly of which type it is.</param>
        /// <returns></returns>
        public string GetHtml(string templateHtml, object model, string user)
        {
            var methodCode = PrepareCSharpCode(templateHtml);
            
            var modelType = model?.GetType() ?? typeof(object);
            var typeName = modelType.FullName;
            
            if (modelType.IsGenericType)
            {
                //typeName = GetGenericTypeFullName(modelType);
                typeName = model.GetType().Name.Replace("`1", string.Empty) + "<" + model.GetType().GenericTypeArguments.First().Name + ">";
            }

            var code = @$"using System;
                                using System.Text;
                                using System.Linq;
                                using System.Collections.Generic;
                                using SIS.MvcFramework;
                                namespace {AppViewNamespace}
                                {{
                                    public class {AppViewCode} : IView
                                    {{
                                        public string GetHtml(object model, string user)
                                        {{
                                            var Model = model as {typeName};
                                            var User = user;
                                            var html = new StringBuilder();

                                {methodCode}

                                            return html.ToString();
                                        }}
                                    }}
                                }}";
            var view = GetInstanceFromCode(code, model);
            var html = view.GetHtml(model, user);
            return html;
        }

        private string GetGenericTypeFullName(Type modelType)
        {
            // ToDo: Add logic 
            return "";
        }

        private IView GetInstanceFromCode(string code, object model)
        {
            /*
                 Create compilable code with generating new Assembly that would result in a new DLL (DynamicallyLinkedLibrary)
                 and add all required references in order for the string that is being passed to be fully operational C# Code
             */
            var compilation = CSharpCompilation.Create(AppViewAssemblyName)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location)) // Add reference to IView based on the location of it's assembly
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location)); // Add top-most libary in C# - System.Object
            
            if (model != null)
            {
                compilation = 
                    compilation.AddReferences(MetadataReference.CreateFromFile(model.GetType().Assembly.Location));
            }
            
            //Add reference to all libraries that NETStandard brings with itself
            var netStandardLibraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();
            foreach (var netStandardLibrary in netStandardLibraries)
            {
                compilation =
                    compilation.AddReferences(
                        MetadataReference.CreateFromFile(Assembly.Load(netStandardLibrary).Location));
            }

            // SyntaxTree has implicit conversion from SourceText to string.
            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));

            //compilation.Emit("temp.dll"); // Save newly created C# Code from our custom assembly in temp.dll. Can be found in the bin directory when project is built
            using var memoryStream = new MemoryStream();

            var compilationResult = compilation.Emit(memoryStream);
            if (!compilationResult.Success)
            {
                return new ErrorView(
                    compilationResult.Diagnostics
                        .Where(d => d.Severity == DiagnosticSeverity.Error)
                        .Select(e => e.GetMessage()));
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            
            var assemblyByteArray = memoryStream.ToArray();
            var assembly = Assembly.Load(assemblyByteArray);
            var type = assembly.GetType($"{AppViewNamespace}.{AppViewCode}");
            var instance = Activator.CreateInstance(type) as IView;
            return instance;
        }
        

        private static string PrepareCSharpCode(string templateHtml)
        {
            const string CSharpRegexExpression = @"[^\<\""\s&]+";
            var regex = new Regex(CSharpRegexExpression, RegexOptions.Compiled);
            
            var supportedOpperators = new[] { "if", "for", "foreach", "else" };
            var cSharpCode = new StringBuilder();
            var reader = new StringReader(templateHtml);
            string htmlLine;
            while ((htmlLine = reader.ReadLine())!= null)
            {
                if (htmlLine.TrimStart().StartsWith("{")
                    || htmlLine.TrimStart().StartsWith("}"))
                {
                    cSharpCode.AppendLine(htmlLine);
                }
                else if (supportedOpperators.Any(x => htmlLine.TrimStart().StartsWith("@" + x)))
                {
                    var indexOfAt = htmlLine.IndexOf("@");
                    htmlLine = htmlLine.Remove(indexOfAt, 1);
                    cSharpCode.AppendLine(htmlLine);
                }
                else
                {
                    // prepend a before the htmlLine in order to ensure that special chars can be escaped
                    var currentLine = new StringBuilder("html.AppendLine(@\"");
                    while (htmlLine.Contains("@"))
                    {
                        var atSignIndex = htmlLine.IndexOf("@");
                        var beforeAtSign = htmlLine.Substring(0, atSignIndex);
                        currentLine.Append(beforeAtSign.Replace("\"", "\"\"") + "\" + ");
                        var cSharpAndEndOfLine = htmlLine.Substring(atSignIndex + 1);
                        var cSharpExpression = regex.Match(cSharpAndEndOfLine);
                        currentLine.Append(cSharpExpression.Value + " + @\"");
                        var afterCSharpCode = cSharpAndEndOfLine.Substring(cSharpExpression.Length);
                        // pass line back to check if there is another @ sign
                        htmlLine = afterCSharpCode;
                    }
                    
                    currentLine.Append(htmlLine.Replace("\"", "\"\"") + "\");");
                    cSharpCode.AppendLine(currentLine.ToString());
                }
            }
            return cSharpCode.ToString();
        }
    }
}