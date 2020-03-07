namespace SulsApp
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=SulsApp;Integrated Security=True;");
        }

        public DbSet<User> Users { get; set; }
        
        public DbSet<Problem> Problems { get; set; }
        
        public DbSet<Submission> Submissions { get; set; }
    }
}