using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YuukoResume.Models
{
    public class Experience
    {
        public long Id { get; set; }

        [Localized]
        [MaxLength(128)]
        public string Company { get; set; }

        [Localized]
        [MaxLength(128)]
        public string Position { get; set; }

        [Localized]
        public string Description { get; set; }

        public DateTime From { get; set; }

        public DateTime? To { get; set; }
    }
}
