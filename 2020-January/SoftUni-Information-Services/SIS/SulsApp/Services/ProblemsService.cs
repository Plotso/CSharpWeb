namespace SulsApp.Services
{
    using Models;

    public class ProblemsService : IProblemsService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProblemsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void CreateProblem(string name, int points)
        {
            var problem = new Problem
            {
                Name = name,
                Points = points
            };
            _dbContext.Problems.Add(problem);
            _dbContext.SaveChanges();
        }
    }
}