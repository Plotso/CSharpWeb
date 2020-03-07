namespace SulsApp.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Submission
    {
        public Submission()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }

        [MaxLength(800)]
        [Required]
        public string Code { get; set; }

        public int AchievedResult { get; set; }
        
        public DateTime CreatedOn { get; set; }

        public string ProblemId { get; set; }
        
        public virtual Problem Problem { get; set; }
        
        public string UserId { get; set; }
        
        public virtual User User { get; set; }
    }
}