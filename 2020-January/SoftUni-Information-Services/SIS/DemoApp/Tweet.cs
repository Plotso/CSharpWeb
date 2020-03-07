namespace DemoApp
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Tweet
    {
        public int Id { get; set; }
        
        [Required]
        public string Creator { get; set; }
        
        [Required]
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }
    }
}