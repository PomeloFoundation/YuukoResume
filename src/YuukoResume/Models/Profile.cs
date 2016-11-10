using System;
using System.ComponentModel.DataAnnotations;

namespace YuukoResume.Models
{
    public class Profile
    {
        public long Id { get; set; }

        [Localized]
        [MaxLength(64)]
        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }

        [Localized]
        public string SelfIntroduce { get; set; }

        [Localized]
        [MaxLength(128)]
        public string Position { get; set; }

        [MaxLength(128)]
        public string GitHubUrl { get; set; }

        [MaxLength(128)]
        public string BlogUrl { get; set; }

        [MaxLength(128)]
        public string LinkedInUrl { get; set; }
    }
}
