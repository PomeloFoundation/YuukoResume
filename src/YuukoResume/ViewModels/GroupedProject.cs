using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YuukoResume.Models;

namespace YuukoResume.ViewModels
{
    public class GroupedProject
    {
        [Localized]
        public string Catalog { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
