namespace SIS.MvcFramework
{
    using System.IO;
    using System.Runtime.CompilerServices;
    using HTTP.Models;
    using HTTP.Response;

    public abstract class Controller
    {
        private const string UserIdSessionKey = "UserId";
        public HttpRequest Request { get; set; }
        
        /// <summary>
        ///  Return HTMLResponse with the respective View from Views folder in your project. FileName parameter has attribute CallerMemberName and can be skipped. 
        /// </summary>
        /// <param name="viewModel">The view model that should be used by the ViewEngine. By default it's null if nothing is passed.</param>
        /// <param name="fileName">If fileName is not passed, compiler will put calling method name as file name</param>
        protected HttpResponse View<T>(T viewModel = null, [CallerMemberName]string fileName = null)
            where T : class
        {
            var controllerName = GetType().Name.Replace("Controller", string.Empty);
            var htmlBodyFilePath = GetHTMLBodyFilePath(controllerName, fileName);
            
            return ViewByName(htmlBodyFilePath, viewModel);
        }
        
        /// <summary>
        ///  Return HTMLResponse with the respective View from Views folder in your project. FileName parameter has attribute CallerMemberName and can be skipped. 
        /// </summary>
        /// <param name="fileName">If fileName is not passed, compiler will put calling method name as file name</param>
        protected HttpResponse View([CallerMemberName]string fileName = null)
        {
            return View<object>(null, fileName);
        }
        
        /// <summary>
        /// Helper method for returning errors
        /// </summary>
        /// <param name="error">Error message to present</param>
        protected HttpResponse Error(string error)
        {
            return ViewByName("Views/Shared/Error.html", new ErrorViewModel {Error = error});
        }
        
        /// <summary>
        /// Helper method for redirection when returning responses
        /// </summary>
        protected HttpResponse Redirect(string url)
        {
            return new RedirectResponse(url);
        }

        /// <summary>
        /// Used to add the users in the current session data, preventing him from having to login every time
        /// </summary>
        /// <param name="username"></param>
        protected void SignIn(string userId)
        {
            Request.SessionData[UserIdSessionKey] = userId;
        }

        /// <summary>
        /// Removes the user from session data storage. He will have to login next time.
        /// </summary>
        /// <param name="username"></param>
        protected void SignOut()
        {
            Request.SessionData[UserIdSessionKey] = null;
        }

        public string User
            => Request.SessionData.ContainsKey(UserIdSessionKey) ?
                Request.SessionData[UserIdSessionKey] :
                null;
        
        /// <summary>
        /// Returns a specific view based on provided ViewModel and ViewPath
        /// </summary>
        private HttpResponse ViewByName(string viewPath, object viewModel)
        {
            var viewEngine = new ViewEngine();
            
            var htmlBody = File.ReadAllText(viewPath);
            htmlBody = viewEngine.GetHtml(htmlBody, viewModel, User);
            
            var layout = File.ReadAllText("Views/Shared/_Layout.html");
            var html = layout.Replace("@RenderBody()", htmlBody);
            html = viewEngine.GetHtml(html, viewModel, User);
            
            return new HtmlResponse(html);
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