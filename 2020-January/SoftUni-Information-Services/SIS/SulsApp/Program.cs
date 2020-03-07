namespace SulsApp
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Controllers;
    using SIS.HTTP;
    using SIS.HTTP.Enums;
    using SIS.MvcFramework;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await WebHost.StartAsync(new Startup());
        }
    }
}