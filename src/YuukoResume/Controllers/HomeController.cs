using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace YuukoResume.Controllers
{
    public class HomeController : BaseController
    {
        [Route("/")]
        [Route("/lang/{culture}")]
        public async Task<IActionResult> Index()
        {
            ViewBag.ProfessionalSkills = await DB.Skills
                .Where(x => x.Performance == Models.SkillPerformance.Professional)
                .OrderByDescending(x => x.Level)
                .ToListAsync();
            ViewBag.OtherSkills = await DB.Skills
                .Where(x => x.Performance == Models.SkillPerformance.Other)
                .OrderByDescending(x => x.Level)
                .ToListAsync();
            ViewBag.LanguageSkills = await DB.Skills
                .Where(x => x.Performance == Models.SkillPerformance.Language)
                .OrderByDescending(x => x.Level)
                .ToListAsync();
            ViewBag.Educations = await DB.Educations
                .OrderByDescending(x => x.From)
                .ToListAsync();
            ViewBag.Projects = await DB.Projects
                .OrderByDescending(x => x.From)
                .ThenBy(x => x.To)
                .ToListAsync();
            ViewBag.Experiences = await DB.Experiences
                .OrderByDescending(x => x.From)
                .ToListAsync();
            ViewBag.Certificates = await DB.Certificates
                .OrderByDescending(x => x.PRI)
                .ToListAsync();
            ViewBag.Profile = Startup.Profile;
            return View();
        }

        public IActionResult Language(string id)
        {
            Response.Cookies.Append("ASPNET_LANG", id);
            return RedirectToAction("Index", "Home");
        }
    }
}
