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
            return View();
        }
        
        [HttpPost("/Problems/Create")]
        public HttpResponse DoCreate(string name, string points)
        {
            if (name == null || !int.TryParse(points, out int intPoints))
            {
                return new RedirectResponse("/Problems/Create");
            }
            
            _problemsService.CreateProblem(name, intPoints);
            return new RedirectResponse("/");
        }
        
        public HttpResponse Details()
        {
            return View();
        }
    }
}