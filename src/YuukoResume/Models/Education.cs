using System;
using System.ComponentModel.DataAnnotations;

namespace YuukoResume.Models
{
    public class Education
    {
        public long Id { get; set; }

        [Localized]
        [MaxLength(512)]
        public string School { get; set; }

        [Localized]
        public string Profession { get; set; }

        public DateTime From { get; set; }

        public DateTime? To { get; set; }
    }
}
