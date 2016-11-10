using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YuukoResume.Models
{
    public enum SkillPerformance
    {
        Bar,
        Circle
    }

    public class Skill
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public float Level { get; set; }

        public SkillPerformance Performance { get; set; }
    }
}
