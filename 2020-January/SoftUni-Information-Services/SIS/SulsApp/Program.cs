namespace SulsApp
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Controllers;
    using SIS.HTTP;
    using SIS.HTTP.Enums;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var db = new ApplicationDbContext();
            db.Database.EnsureCreated();
            Console.WriteLine("Hello World!");
            
            var routeTable = new List<Route>();
            routeTable.Add(new Route(HttpMethodType.Get, "/", new HomeController().Index));
            routeTable.Add(new Route(HttpMethodType.Get, "/css/bootstrap.min.css", new StaticFilesController().Bootstrap));
            routeTable.Add(new Route(HttpMethodType.Get, "/css/site.css", new StaticFilesController().Site));
            routeTable.Add(new Route(HttpMethodType.Get, "/css/reset.css", new StaticFilesController().Reset));
            routeTable.Add(new Route(HttpMethodType.Get, "/Users/Login", new UsersController().Login));
            routeTable.Add(new Route(HttpMethodType.Get, "/Users/Register", new UsersController().Register));
            
            var httpServer = new HttpServer(80, routeTable);
            await httpServer.StartAsync();
        }
    }
}