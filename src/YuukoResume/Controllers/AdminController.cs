using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace YuukoResume.Controllers
{
    public class AdminController : BaseController
    {
        [HttpGet]
        [GuestOnly]
        public IActionResult Login() => View();

        [HttpPost]
        [GuestOnly]
        public async Task<IActionResult> Login(string Username, string Password)
        {
            if (await DB.Profiles.AnyAsync(x => x.Username == Username && x.Password == Password))
            {
                HttpContext.Session.SetString("Admin", "TRUE");
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
    }
}
