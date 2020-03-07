namespace SulsApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
            Submissions = new HashSet<Submission>();
        }
        
        public string Id { get; set; }
        
        [MaxLength(20)]
        //[MinLength(5)]
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        public virtual ICollection<Submission> Submissions { get; set; }
    }
}