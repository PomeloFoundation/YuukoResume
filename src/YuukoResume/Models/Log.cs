using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YuukoResume.Models
{
    public class Log
    {
        public Guid Id { get; set; }

        public DateTime Time { get; set; }

        public string Email { get; set; }

        public bool IsRead { get; set; }
    }
}
