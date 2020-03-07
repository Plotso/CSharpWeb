namespace SulsApp.Controllers
{
    using System.IO;
    using SIS.HTTP.Models;
    using SIS.HTTP.Response;

    public class UsersController
    {
        public HttpResponse Login(HttpRequest request)
        {
            var layout = File.ReadAllText("Views/Shared/_Layout.html");
            var htmlBody = File.ReadAllText("Views/Users/Login.html");
            var html = layout.Replace("@RenderBody()", htmlBody);
            
            return new HtmlResponse(html);
        }
        
        public HttpResponse Register(HttpRequest request)
        {
            var layout = File.ReadAllText("Views/Shared/_Layout.html");
            var htmlBody = File.ReadAllText("Views/Users/Register.html");
            var html = layout.Replace("@RenderBody()", htmlBody);
            
            return new HtmlResponse(html);
        }
    }
}