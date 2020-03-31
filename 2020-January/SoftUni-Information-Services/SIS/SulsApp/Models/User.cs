namespace SulsApp.Models
{
    using System;
    using System.Collections.Generic;
    using SIS.MvcFramework.Authentication;

    public class User : IdentityUser<string>
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
            Submissions = new HashSet<Submission>();
        }
        
        public virtual ICollection<Submission> Submissions { get; set; }
    }
}