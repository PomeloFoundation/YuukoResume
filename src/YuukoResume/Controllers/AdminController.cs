using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YuukoResume.Models;
using YuukoResume.Filters;

namespace YuukoResume.Controllers
{
    public class AdminController : BaseController
    {
        public override async void Prepare()
        {
            base.Prepare();

            ViewBag.Notifications = await DB.Logs
                .Where(x => !x.IsRead)
                .OrderBy(x => x.Time)
                .ToListAsync();
        }

        #region Login & Logout
        [HttpGet]
        [GuestOnly]
        public IActionResult Login() => View();

        [HttpPost]
        [GuestOnly]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Username, string Password)
        {
            if (Startup.Profile.Username == Username && Startup.Profile.Password == Password)
            {
                HttpContext.Session.SetString("Admin", "TRUE");
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Dashboard & Profile Management
        [HttpGet]
        [AdminRequired]
        public IActionResult Index() => View();

        [HttpGet]
        [AdminRequired]
        public IActionResult Profile() => View();

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(Profile Model)
        {
            Startup.Profile = Model;
            System.IO.File.WriteAllText("profile.json", JsonConvert.SerializeObject(Model));
            return Prompt(x => 
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Your profile has been saved successfully."];
            });
        }
        #endregion

        #region Project Management
        [HttpGet]
        [AdminRequired]
        public IActionResult Project() => PagedView(DB.Projects.OrderByDescending(x => x.From).ThenBy(x => x.To), 10);

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Project/Remove/{id}")]
        public async Task<IActionResult> RemoveProject(long id)
        {
            await DB.Projects
                .Where(x => x.Id == id)
                .DeleteAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = "The project has been removed successfully.";
            });
        }

        [HttpGet]
        [AdminRequired]
        [Route("[controller]/Project/Edit/{id}")]
        public async Task<IActionResult> EditProject(long id) => View(await DB.Projects.SingleAsync(x => x.Id == id));

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Project/Edit/{id}")]
        public async Task<IActionResult> EditProject(long id, Project Model)
        {
            await DB.Projects
                .Where(x => x.Id == id)
                .SetField(x => x.BlobId).WithValue(Model.BlobId)
                .SetField(x => x.Catalog).WithValue(Model.Catalog)
                .SetField(x => x.DemoUrl).WithValue(Model.DemoUrl)
                .SetField(x => x.Description).WithValue(Model.Description)
                .SetField(x => x.From).WithValue(Model.From)
                .SetField(x => x.To).WithValue(Model.To)
                .SetField(x => x.GitHub).WithValue(Model.GitHub)
                .SetField(x => x.Tags).WithValue(Model.Tags)
                .SetField(x => x.Title).WithValue(Model.Title)
                .UpdateAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Project info has been saved successfully."];
            });
        }

        [HttpGet]
        [AdminRequired]
        [Route("[controller]/Project/Create")]
        public IActionResult CreateProject() => View();

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Project/Create")]
        public async Task<IActionResult> CreateProject(Project Model)
        {
            DB.Projects.Add(Model);
            await DB.SaveChangesAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Project info has been created successfully."];
            });
        }
        #endregion

        #region Education Management
        [HttpGet]
        [AdminRequired]
        public async Task<IActionResult> Education() => View(await DB.Educations.OrderByDescending(x => x.From).ThenBy(x => x.To).ToListAsync());

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Education/Create")]
        public async Task<IActionResult> CreateEducation(Education Model)
        {
            DB.Educations.Add(Model);
            await DB.SaveChangesAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Education information has been created successfully."];
            });
        }

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Education/Remove/{id}")]
        public async Task<IActionResult> RemoveEducation(long id)
        {
            await DB.Educations
                .Where(x => x.Id == id)
                .DeleteAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Education information has been removed successfully."];
            });
        }
        #endregion

        #region Skill Management
        [HttpGet]
        [AdminRequired]
        public async Task<IActionResult> Skill() => View(await DB.Skills.GroupBy(x => x.Performance).Select(x => new { Performance = x.Key, Skills = x.OrderByDescending(y => y.Level) }).ToListAsync());
        
        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Skill/Create")]
        public async Task<IActionResult> CreateSkill(Skill Model)
        {
            DB.Skills.Add(Model);
            await DB.SaveChangesAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Skill has been created successfully."];
            });
        }


        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Skill/Remove/{id}")]
        public async Task<IActionResult> RemoveSkill(long id)
        {
            await DB.Skills
                .Where(x => x.Id == id)
                .DeleteAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Skill has been removed successfully."];
            });
        }

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Skill/Edit/{id}")]
        public async Task<IActionResult> EditSkill(long id, float level)
        {
            await DB.Skills
                .Where(x => x.Id == id)
                .SetField(x => x.Level).WithValue(level)
                .UpdateAsync();
            return Prompt(x => 
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Skill has been saved successfully."];
            });
        }
        #endregion

        #region Certificate Management
        [HttpGet]
        [AdminRequired]
        public async Task<IActionResult> Certificate() => View(await DB.Certificates.OrderByDescending(x => x.PRI).ToListAsync());

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Certificate/Create")]
        public async Task<IActionResult> CreateCertificate(Certificate Model)
        {
            DB.Certificates.Add(Model);
            await DB.SaveChangesAsync();
            return Prompt(x => 
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Certificate has been created successfully."];
            });
        }

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Certificate/Remove/{id}")]
        public async Task<IActionResult> RemoveCertificate(long id)
        {
            await DB.Certificates
                .Where(x => x.Id == id)
                .DeleteAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Certificate has been removed successfully."];
            });
        }
        #endregion

        #region Experience Management
        #endregion
    }
}
