namespace SulsApp.Controllers
{
    using System.IO;
    using SIS.HTTP.Models;
    using SIS.HTTP.Response;

    public class HomeController
    {
        public HttpResponse Index(HttpRequest request)
        {
            var layout = File.ReadAllText("Views/Shared/_Layout.html");
            var htmlBody = File.ReadAllText("Views/Home/Index.html");
            var html = layout.Replace("@RenderBody()", htmlBody);
            
            return new HtmlResponse(html);
        }
    }
}