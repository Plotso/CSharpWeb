namespace SulsApp.Services
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Models;
    using SIS.MvcFramework.Authentication;

    public class UsersService : IUsersService
    {
        private ApplicationDbContext _db;
        
        public UsersService(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        
        public void CreateUser(string username, string email, string password)
        {
            var user = new User()
            {
                Email = email,
                Username = username,
                Password = Hash(password),
                Role = IdentityRole.User
            };
            
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public string GetUserId(string username, string password)
        {
            var passwordHash = Hash(password);
            return _db.Users
                .Where(u => u.Username == username && u.Password == passwordHash)
                .Select(u => u.Id)
                .FirstOrDefault();
        }

        public bool IsUsernameUsed(string username)
        {
            return _db.Users.Any(u => u.Username == username);
        }

        public bool IsEmailUsed(string email)
        {
            return _db.Users.Any(u => u.Email == email);
        }

        public void ChangePassword(string username, string newPassword)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return;
            }

            user.Password = Hash(newPassword);
            _db.SaveChanges();
        }

        public int CountUsers()
        {
            return _db.Users.Count();
        }
        
        private string Hash(string input)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2")); // convert byte from symbols to string
            }
            return hash.ToString();
        }
    }
}