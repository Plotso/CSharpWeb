namespace SulsApp
{
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Services;
    using SIS.HTTP;
    using SIS.MvcFramework;
    using SIS.MvcFramework.DI;

    public class Startup : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();
        }
        
        public void Configure(IList<Route> routeTable)
        {
            /*
             moved to autoregistration inside the WebHost
            //routeTable.Add(new Route(HttpMethodType.Get, "/", new HomeController().Index));
            // routeTable.Add(new Route(HttpMethodType.Get, "/css/bootstrap.min.css", new StaticFilesController().Bootstrap));
            // routeTable.Add(new Route(HttpMethodType.Get, "/css/site.css", new StaticFilesController().Site));
            // routeTable.Add(new Route(HttpMethodType.Get, "/css/reset.css", new StaticFilesController().Reset));
            //routeTable.Add(new Route(HttpMethodType.Get, "/Users/Login", new UsersController().Login));
            //routeTable.Add(new Route(HttpMethodType.Get, "/Users/Register", new UsersController().Register));
            */
            
            var db = new ApplicationDbContext();
            db.Database.Migrate();
        }
    }
}