namespace SulsApp.Controllers
{
    using System;
    using SIS.HTTP.Models;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes;
    using ViewModels;

    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
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