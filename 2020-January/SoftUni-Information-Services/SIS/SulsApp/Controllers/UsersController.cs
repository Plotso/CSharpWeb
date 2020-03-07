namespace SulsApp.Controllers
{
    using SIS.HTTP.Models;
    using SIS.MvcFramework;

    public class UsersController : Controller
    {
        public HttpResponse Login(HttpRequest request)
        {
            return View();
        }
        
        public HttpResponse Register(HttpRequest request)
        {
            return View();
        }
    }
}