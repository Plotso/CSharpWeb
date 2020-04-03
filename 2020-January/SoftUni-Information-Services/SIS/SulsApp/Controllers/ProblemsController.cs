namespace SulsApp.Controllers
{
    using Services;
    using SIS.HTTP.Models;
    using SIS.HTTP.Response;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes;

    public class ProblemsController : Controller
    {
        private readonly IProblemsService _problemsService;

        public ProblemsController(IProblemsService problemsService)
        {
            _problemsService = problemsService;
        }

        public HttpResponse Create()
        {
            if (!IsUserLoggedIn())
            {
                return Redirect("/Users/Login");
            }
            return View();
        }
        
        [HttpPost]
        public HttpResponse Create(string name, int points)
        {
            if (!IsUserLoggedIn())
            {
                return Redirect("/Users/Login");
            }
            
            if (string.IsNullOrEmpty(name))
            {
                return Error("Invalid name");
            }

            if (points < 0 || points > 100)
            {
                return Error("Points not in range [0, 100]");
            }
            
            _problemsService.CreateProblem(name, points);
            return new RedirectResponse("/");
        }
        
        public HttpResponse Details()
        {
            return View();
        }
    }
}