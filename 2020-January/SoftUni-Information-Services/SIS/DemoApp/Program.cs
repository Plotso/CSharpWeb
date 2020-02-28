namespace DemoApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using SIS.HTTP;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Models;
    using SIS.HTTP.Response;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var routeTable = new List<Route>();
            routeTable.Add(new Route(HttpMethodType.Get, "/", Index));
            routeTable.Add(new Route(HttpMethodType.Get, "/users/login", Login));
            routeTable.Add(new Route(HttpMethodType.Post, "/users/login", DoLogin));
            routeTable.Add(new Route(HttpMethodType.Get, "/contact", Contact));
            //routeTable.Add(new Route(HttpMethodType.Get, "/favicon.ico", FavIcon));
            
            var httpServer = new HttpServer(80, routeTable);
            await httpServer.StartAsync();
            Console.WriteLine("Hello World!");
        }

        public static HttpResponse Index(HttpRequest request)
        {
            return new HtmlResponse("<h1> Home Page </h1>");
        }
        
        public static HttpResponse Login(HttpRequest request)
        {
            return new HtmlResponse("<h1> Login Page </h1>");
        }
        
        public static HttpResponse DoLogin(HttpRequest request)
        {
            return new HtmlResponse("<h1> Login Form to be added </h1>");
        }
        
        public static HttpResponse Contact(HttpRequest request)
        {
            return new HtmlResponse("<h1> Contact Page </h1>");
        }
        
        private static HttpResponse FavIcon(HttpRequest request)
        {
            throw new NotImplementedException();
            var byteContent = File.ReadAllBytes("wwwroot/favicon.ico");
            //return new FileResponse(byteContent, "image/x-icon");
        }
    }
}