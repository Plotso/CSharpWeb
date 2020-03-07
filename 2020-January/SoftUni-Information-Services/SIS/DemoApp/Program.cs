namespace DemoApp
{
    using System.Threading.Tasks;
    using SIS.MvcFramework;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            await WebHost.StartAsync(new Startup());
        }
    }
}