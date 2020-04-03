namespace SulsApp.Controllers
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using Services;
    using SIS.HTTP.Logging;
    using SIS.HTTP.Models;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes;
    using ViewModels.Users;

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
        public HttpResponse DoLogin(string username, string password)
        {
            if (username == null || password == null)
            {
                return Redirect("/Users/Login");
            }
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
        public HttpResponse DoRegister(RegisterInputModel input)
        {
            if (input == null)
            {
                return Redirect("/Users/Register");
            }

            if (input.Password != input.ConfirmPassword)
            {
                return Error("Passwords should be the same!");
            }

            if (input.Username?.Length < 5 || input.Username?.Length > 20)
            {
                return Error("Username should be at between 5 and 20 characters!");
            }

            if (input.Password?.Length < 6 || input.Password?.Length > 20)
            {
                return Error("Password should be at between 6 and 20 characters");
            }

            if (!IsValid(input.Email))
            {
                return Error("Invalid email!");
            }

            _usersService.CreateUser(input.Username, input.Email, input.Password);
            _logger.Log("New user: " + input.Username);

            return Redirect("/Users/Login");
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