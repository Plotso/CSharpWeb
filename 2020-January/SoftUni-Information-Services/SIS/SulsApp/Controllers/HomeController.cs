namespace SulsApp.Controllers
{
    using System;
    using SIS.HTTP.Models;
    using SIS.MvcFramework;
    using ViewModels;

    public class HomeController : Controller
    {
        public HttpResponse Index(HttpRequest request)
        {
            var viewModel = new IndexViewModel
            {
                Message = "Welcome to SULS Platform!",
                Year = DateTime.UtcNow.Year
            };
            return View(viewModel);
        }
    }
}