namespace DemoApp
{
    using System;
    using System.Threading.Tasks;
    using SIS.HTTP;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Actions:
            // => IndexPage()
            // /favicon.ico => favicon.ico
            // GET /Contact => response ShowContactForm(request)
            // POST /Contact => response FillContactForm(request)
            //new HttpServer(80,
            
            var httpServer = new HttpServer(80);
            await httpServer.StartAsync();
            Console.WriteLine("Hello World!");
        }
    }
}