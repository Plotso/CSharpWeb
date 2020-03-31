namespace SulsApp.Controllers
{
    using System;
    using System.Net.Mail;
    using Services;
    using SIS.HTTP.Logging;
    using SIS.HTTP.Models;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes;

    public class UsersController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUsersService _usersService;
        public UsersController(IUsersService usersService, ILogger logger)
        {
            _logger = logger;
            _usersService = usersService;
        }
        
        public HttpResponse Login()
        {
            return View();
        }

        [HttpPost("/Users/Login")]
        public HttpResponse DoLogin()
        {
            var requestData = Request.FormData;

            var username = requestData["username"];
            var password = requestData["password"];

            var userId = _usersService.GetUserId(username, password);
            if (userId == null)
            {
                return Redirect("/Users/Login");
            }
            
            SignIn(userId);
            _logger.Log("User logged in: " + username);
            return Redirect("/");
        }
        
        public HttpResponse Register()
        {
            return View();
        }

        [HttpPost("/Users/Register")]
        public HttpResponse DoRegister()
        {
            var requestData = Request.FormData;
            
            var username = requestData["username"];
            var email = requestData["email"];
            var password = requestData["password"];
            var confirmPassword = requestData["confirmPassword"];

            if (password != confirmPassword)
            {
                return Error("Passwords should be the same!");
            }

            if (username?.Length < 5 || username?.Length > 20)
            {
                return Error("Username should be at between 5 and 20 characters!");
            }

            if (password?.Length < 6 || password?.Length > 20)
            {
                return Error("Password should be at between 6 and 20 characters");
            }

            if (!IsValid(email))
            {
                return Error("Invalid email!");
            }

            _usersService.CreateUser(username, email, password);
            _logger.Log("New user: " + username);

            return Redirect("Users/Login");
        }

        public HttpResponse Logout()
        {
            SignOut();
            return Redirect("/");
        }
        
        private bool IsValid(string emailaddress)
        {
            try
            {
                new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}