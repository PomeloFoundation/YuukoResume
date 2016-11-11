using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YuukoResume.Models
{
    public class Profile
    {
        public long Id { get; set; }
        
        public Dictionary<string, string> Name { get; set; }

        public DateTime Birthday { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }

        public Dictionary<string, string> SelfIntroduce { get; set; }

        public Dictionary<string, string> Position { get; set; }
        
        public string GitHubUrl { get; set; }
        
        public string BlogUrl { get; set; }
        
        public string LinkedInUrl { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string SmtpServer { get; set; }
        
        public string SmtpUsername { get; set; }
        
        public string SmtpPassword { get; set; }

        public int SmtpPort { get; set; }

        public bool SmtpSSL { get; set; }
    }
}
