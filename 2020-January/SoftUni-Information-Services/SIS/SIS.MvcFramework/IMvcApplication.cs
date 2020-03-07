namespace SIS.MvcFramework
{
    using System.Collections.Generic;
    using HTTP;

    public interface IMvcApplication
    {
        void Configure(IList<Route> routeTable);

        void ConfigureServices();
    }
}