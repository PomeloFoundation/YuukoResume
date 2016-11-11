using System.Collections.Generic;
using YuukoResume.Models;

namespace YuukoResume.ViewModels
{
    public class GroupedProject
    {
        public string Catalog { get; set; }
        public IEnumerable<Project> Projects { get; set; }
    }
}
