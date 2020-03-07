namespace SulsApp.Controllers
{
    using SIS.HTTP.Models;
    using SIS.MvcFramework;

    public class HomeController : Controller
    {
        public HttpResponse Index(HttpRequest request)
        {
            return View();
        }
    }
}