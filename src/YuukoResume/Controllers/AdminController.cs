using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var project = await DB.Projects.SingleAsync(x => x.Id == id);
            project.BlobId = Model.BlobId;
            project.Catalog = Model.Catalog;
            project.DemoUrl = Model.DemoUrl;
            project.Description = Model.Description;
            project.From = Model.From;
            project.To = Model.To;
            project.GitHub = Model.GitHub;
            project.Tags = Model.Tags;
            project.Title = Model.Title;
            await DB.SaveChangesAsync();
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
    }
}
