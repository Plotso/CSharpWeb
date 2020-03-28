namespace SulsApp.Controllers
{
    using System;
    using System.IO;
    using SIS.HTTP.Models;
    using SIS.HTTP.Response;
    using SIS.MvcFramework;

    [Obsolete("Static files registration is handled out of the box inside the WebHost.cs in the MvcFramework project. Every file inside wwwroot folder would be automatically registered!")]
    public class StaticFilesController : Controller
    {
        private const string CSSContentType = "text/css";
        private const string CSSDirectory = "wwwroot/css";
            
        public HttpResponse Bootstrap(HttpRequest request)
        {
            return new FileResponse(File.ReadAllBytes(CSSDirectory + "/bootstrap.min.css"), CSSContentType);
        }
        
        public HttpResponse Reset(HttpRequest request)
        {
            return new FileResponse(File.ReadAllBytes(CSSDirectory + "/reset-css.css"), CSSContentType);
        }
        
        public HttpResponse Site(HttpRequest request)
        {
            return new FileResponse(File.ReadAllBytes(CSSDirectory + "/site.css"), CSSContentType);
        }
    }
}