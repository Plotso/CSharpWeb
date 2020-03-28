namespace SIS.MvcFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Attributes;
    using HTTP;
    using HTTP.Enums;
    using HTTP.Models;
    using HTTP.Response;

    public class WebHost
    {
        public static async Task StartAsync(IMvcApplication application)
        {
            var routeTable = new List<Route>();
            application.ConfigureServices();
            application.Configure(routeTable);
            AutoRegisterStaticFilesRoutes(routeTable);
            AutoRegisterActionRoutes(routeTable, application);
            
            Console.WriteLine("Routes:");
            foreach (var route in routeTable)
            {
                Console.WriteLine(route.ToString());
            }

            Console.WriteLine("Requests:");
            var httpServer = new HttpServer(80, routeTable);
            await httpServer.StartAsync();
        }


        /// <summary>
        /// AutoRegistration of all actions of the user application. Actions are found in the following pattern - /{controller}/{action}/
        /// </summary>
        private static void AutoRegisterActionRoutes(List<Route> routeTable, IMvcApplication application)
        {
            // Can also used Assembly.GetEntryAssembly().GetTypes()
            var controllers = application.GetType().Assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Controller)) && !type.IsAbstract);

            foreach (var controller in controllers)
            {
                var actions = controller.GetMethods()
                    .Where(m =>
                            !m.IsSpecialName &&
                            !m.IsConstructor &&
                            m.IsPublic &&
                            m.DeclaringType ==
                            controller //filter method declared only be this class. Can also use != typeof(object) if the class isn't inheriting from any other
                    );
                foreach (var action in actions)
                {
                    var url = "/" + controller.Name.Replace("Controller", string.Empty) + "/" + action.Name;

                    var attribute = action.GetCustomAttributes()
                            .FirstOrDefault(a => a
                                .GetType()
                                .IsSubclassOf(typeof(HttpMethodAttribute)))
                        as HttpMethodAttribute;
                    var httpActionType = HttpMethodType.Get;
                    if (attribute != null)
                    {
                        httpActionType = attribute.Type;
                        if (attribute.Url != null)
                        {
                            url = attribute.Url;
                        }
                    }
                    
                    //ToDo: Implement logic for returning valid Response based on the action above
                    routeTable.Add(new Route(httpActionType, url, (request) =>
                    {
                        var controllerInstance = Activator.CreateInstance(controller) as Controller;
                        var response = action.Invoke(controllerInstance, new [] {request}) as HttpResponse;
                        return response;
                        //return new HtmlResponse("Not yet implemented");
                    }));
                    Console.WriteLine(attribute?.ToString());
                }
            }
        }


        /// <summary>
        /// AutoRegistration of all static files inside wwwroot folder of the user application
        /// </summary>
        private static void AutoRegisterStaticFilesRoutes(List<Route> routeTable)
        {
            //since this dll will be used inside another application after compilation, it's not a problem that "wwwroot" folder is not present inside SIS.MvcFramework library
            var staticFiles = Directory.GetFiles("wwwroot", "*", SearchOption.AllDirectories);

            foreach (var staticFile in staticFiles)
            {
                var path = staticFile.Replace("wwwroot", string.Empty).Replace("\\", "/");

                routeTable.Add(new Route(HttpMethodType.Get, path, (request) =>
                {
                    var fileInfo = new FileInfo(staticFile);
                    var contentType = fileInfo.Extension switch
                    {
                        ".css" => "text/css",
                        ".html" => "text/html",
                        ".js" => "text/javascript",
                        ".ico" => "image/x-icon",
                        ".jpg" => "image/jpeg",
                        ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        _ => "text/plain",
                    };

                    return new FileResponse(File.ReadAllBytes(staticFile), contentType);
                }));
            }
        }
    }
}