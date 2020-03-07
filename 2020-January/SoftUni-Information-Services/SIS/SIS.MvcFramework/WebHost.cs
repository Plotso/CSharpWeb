namespace SIS.MvcFramework
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HTTP;

    public class WebHost
    {
        public static async Task StartAsync(IMvcApplication application)
        {
            var routeTable = new List<Route>();
            application.ConfigureServices();
            application.Configure(routeTable);
            
            var httpServer = new HttpServer(80, routeTable);
            await httpServer.StartAsync();
        }
    }
}