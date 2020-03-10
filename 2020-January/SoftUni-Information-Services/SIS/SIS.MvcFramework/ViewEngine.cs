namespace SIS.MvcFramework
{
    using System;
    using System.IO;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Emit;

    public class ViewEngine : IViewEngine
    {
        private const string AppViewAssemblyName = "AppViewAssembly";
        private const string AppViewNamespace = "AppViewNamespace";
        private const string AppViewCode = "AppViewCode";
        
        public string GetHtml(string templateHtml, object model)
        {
            var methodCode = PrepareCSharpCode(templateHtml);

            var code = @$"using System;
                                using System.Text;
                                using System.Linq;
                                using System.Collections.Generic;
                                using SIS.MvcFramework;
                                namespace {AppViewNamespace}
                                {{
                                    public class {AppViewCode} : IView
                                    {{
                                        public string GetHtml(object model)
                                        {{
                                            //var User = user;
                                            var html = new StringBuilder();

                                {methodCode}

                                            return html.ToString();
                                        }}
                                    }}
                                }}";
            //IView view = GetInstanceFromCode(code, model);
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
                //ToDo: Add functionality to return ErrorView
                return null;
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            
            var assemblyByteArray = memoryStream.ToArray();
            var assembly = Assembly.Load(assemblyByteArray);
            var type = assembly.GetType($"{AppViewNamespace}.{AppViewCode}");
            var instance = Activator.CreateInstance(type) as IView;
            return instance;
        }
        

        private string PrepareCSharpCode(string templateHtml)
        {
            //ToDo: Add functionality
            return "";
        }
    }
}