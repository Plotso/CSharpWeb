namespace DemoApp
{
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=DemoApp;Integrated Security=True;");
        }

        public DbSet<Tweet> Tweets { get; set; }
    }
}