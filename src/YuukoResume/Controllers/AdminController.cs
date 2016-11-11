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

        #region Profile Management
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
    }
}
