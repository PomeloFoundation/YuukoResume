using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace YuukoResume.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.SkillsBar = DB.Skills
                .Where(x => x.Performance == Models.SkillPerformance.Bar)
                .OrderByDescending(x => x.Level)
                .ToList();
            ViewBag.SkillsCircle = DB.Skills
                .Where(x => x.Performance == Models.SkillPerformance.Circle)
                .OrderByDescending(x => x.Level)
                .ToList();
            ViewBag.Educations = DB.Educations
                .OrderByDescending(x => x.From)
                .ToList();
            ViewBag.Projects = DB.Projects
                .GroupBy(x => x.Catalog)
                .Select(x => new {
                    Catalog = x.Key,
                    Projects = x.OrderByDescending(y => y.From)
                })
                .OrderBy(x => x.Catalog)
                .ToList();
            ViewBag.Experiences = DB.Experiences
                .OrderByDescending(x => x.From)
                .ToList();
            ViewBag.Certificates = DB.Certificates
                .OrderByDescending(x => x.PRI)
                .ToList();
            ViewBag.Profile = DB.Profiles.First();
            return View();
        }

        public IActionResult Language(string id)
        {
            Response.Cookies.Append("ASPNET_LANG", id);
            return RedirectToAction("Index", "Home");
        }
    }
}
