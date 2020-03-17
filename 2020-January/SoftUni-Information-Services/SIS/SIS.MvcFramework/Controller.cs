namespace SIS.MvcFramework
{
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using HTTP.Models;
    using HTTP.Response;

    public abstract class Controller
    {
        /// <summary>
        ///  Return HTMLResponse with the respective View from Views folder in your project. FileName parameter has attribute CallerMemberName and can be skipped. 
        /// </summary>
        /// <param name="viewModel">The view model that should be used by the ViewEngine. By default it's null if nothing is passed.</param>
        /// <param name="fileName">If fileName is not passed, compiler will put calling method name as file name</param>
        protected HttpResponse View<T>(T viewModel = null, [CallerMemberName]string fileName = null)
            where T : class
        {
            var viewEngine = new ViewEngine();
            var controllerName = GetType().Name.Replace("Controller", string.Empty);
            var htmlBodyFilePath = GetHTMLBodyFilePath(controllerName, fileName);
            
            var htmlBody = File.ReadAllText(htmlBodyFilePath);
            htmlBody = viewEngine.GetHtml(htmlBody, viewModel);
            
            var layout = File.ReadAllText("Views/Shared/_Layout.html");
            var html = layout.Replace("@RenderBody()", htmlBody);
            html = viewEngine.GetHtml(html, viewModel);
            
            return new HtmlResponse(html);
        }
        
        /// <summary>
        ///  Return HTMLResponse with the respective View from Views folder in your project. FileName parameter has attribute CallerMemberName and can be skipped. 
        /// </summary>
        /// <param name="fileName">If fileName is not passed, compiler will put calling method name as file name</param>
        protected HttpResponse View([CallerMemberName]string fileName = null)
        {
            return this.View<object>(null, fileName);
        }

        /// <summary>
        /// HTMLBody file locations would be for example Views/Home/Index.html
        /// This would be executed from Index method in the HomeController, passing Index.html
        /// That's why, this whole path can be taken via reflection to actually obtain full path to the view following ASP.NET Core conventions
        /// </summary>
        private static string GetHTMLBodyFilePath(string controllerName, string fileName) 
            => $"Views/{controllerName}/" + fileName + ".html";
    }
}