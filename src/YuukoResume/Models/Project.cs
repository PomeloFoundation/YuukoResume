using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YuukoResume.Models
{
    public class Project
    {
        public long Id { get; set; }

        [MaxLength(64)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime From { get; set; }

        public DateTime? To { get; set; }
    }
}
