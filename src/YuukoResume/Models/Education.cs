using System;
using System.ComponentModel.DataAnnotations;

namespace YuukoResume.Models
{
    public class Education
    {
        public long Id { get; set; }

        [MaxLength(64)]
        public string School { get; set; }

        public string Profession { get; set; }

        public DateTime From { get; set; }

        public DateTime? To { get; set; }
    }
}
