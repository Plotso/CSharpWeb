namespace SIS.MvcFramework
{
    using System.Collections.Generic;
    using DI;
    using HTTP;

    public interface IMvcApplication
    {
        void Configure(IList<Route> routeTable);

        void ConfigureServices(IServiceCollection serviceCollection);
    }
}