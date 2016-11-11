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
        public IActionResult Index()
        {
            Parallel.Invoke(async ()=> 
            {
                ViewBag.SkillsBar = await DB.Skills
                    .Where(x => x.Performance == Models.SkillPerformance.Bar)
                    .OrderByDescending(x => x.Level)
                    .ToListAsync();
            }, 
            async ()=>
            {
                ViewBag.SkillsCircle = await DB.Skills
                    .Where(x => x.Performance == Models.SkillPerformance.Circle)
                    .OrderByDescending(x => x.Level)
                    .ToListAsync();
            },
            async () => 
            {
                ViewBag.Educations = await DB.Educations
                    .OrderByDescending(x => x.From)
                    .ToListAsync();
            },
            async () =>
            {
                ViewBag.Projects = await DB.Projects
                    .GroupBy(x => x.Catalog)
                    .Select(x => new
                    {
                        Catalog = x.Key,
                        Projects = x.OrderByDescending(y => y.From)
                    })
                    .OrderBy(x => x.Catalog)
                    .ToListAsync();
            },
            async () =>
            {
                ViewBag.Experiences = await DB.Experiences
                    .OrderByDescending(x => x.From)
                    .ToListAsync();
            },
            async () =>
            {
                ViewBag.Certificates = await DB.Certificates
                    .OrderByDescending(x => x.PRI)
                    .ToListAsync();
            },
            async () =>
            {
                ViewBag.Profile = await DB.Profiles.FirstAsync();
            });
            return View();
        }

        public IActionResult Language(string id)
        {
            Response.Cookies.Append("ASPNET_LANG", id);
            return RedirectToAction("Index", "Home");
        }
    }
}
