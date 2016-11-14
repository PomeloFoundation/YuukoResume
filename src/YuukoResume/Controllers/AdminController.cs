using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pomelo.AspNetCore.Localization;
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

        #region Login, Logout & Languages
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

        [AdminRequired]
        public IActionResult Language(string id, [FromHeader] string Referer)
        {
            Response.Cookies.Append("ASPNET_LANG", id);
            return Redirect(Referer);
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
        public async Task<IActionResult> Profile(Profile Model, string _name, string _position, string _selfIntroduce, IFormFile Avatar, [FromServices] ICultureProvider cultureProvider)
        {
            var key = cultureProvider.DetermineCulture();
            Model.Name = Startup.Profile.Name;
            Model.Position = Startup.Profile.Position;
            Model.SelfIntroduce = Startup.Profile.SelfIntroduce;
            if (Model.Name.ContainsKey(key))
                Model.Name.Remove(key);
            if (Model.Position.ContainsKey(key))
                Model.Position.Remove(key);
            if (Model.SelfIntroduce.ContainsKey(key))
                Model.SelfIntroduce.Remove(key);
            Model.Name.Add(key, _name);
            Model.Position.Add(key, _position);
            Model.SelfIntroduce.Add(key, _selfIntroduce);
            if (Avatar != null && Avatar.Length > 0)
            {
                var blob = new Pomelo.AspNetCore.Extensions.BlobStorage.Models.Blob
                {
                    Bytes = await Avatar.ReadAllBytesAsync(),
                    FileName = Avatar.FileName,
                    ContentLength = Avatar.Length,
                    Time = DateTime.Now,
                    ContentType = Avatar.ContentType
                };
                DB.Blobs.Add(blob);
                DB.SaveChanges();
                Model.AvatarId = blob.Id;
            }
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
            var project = await DB.Projects.SingleAsync(x => x.Id == id);
            DB.Projects.Remove(project);
            await DB.SaveChangesAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = "The project has been removed successfully.";
                x.HideBack = true;
                x.RedirectText = SR["Back To Project List"];
                x.RedirectUrl = Url.Action("Project", "Admin");
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
                x.HideBack = true;
                x.RedirectText = SR["Back To Project List"];
                x.RedirectUrl = Url.Action("Project", "Admin");
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
                x.HideBack = true;
                x.RedirectText = SR["Back To Project List"];
                x.RedirectUrl = Url.Action("Project", "Admin");
            });
        }
        #endregion

        #region Education Management
        [HttpGet]
        [AdminRequired]
        public async Task<IActionResult> Education() => View(await DB.Educations.OrderByDescending(x => x.From).ThenBy(x => x.To).ToListAsync());

        [HttpGet]
        [AdminRequired]
        [Route("[controller]/Education/Create")]
        public IActionResult CreateEducation() => View();

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Education/Create")]
        public async Task<IActionResult> CreateEducation(Education Model, [FromServices] ResumeContext DB)
        {
            DB.Educations.Add(Model);
            var entry = DB.Entry(Model);
            await DB.SaveChangesAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Education experience has been created successfully."];
                x.HideBack = true;
                x.RedirectText = SR["Back To Education List"];
                x.RedirectUrl = Url.Action("Education", "Admin");
            });
        }

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Education/Remove/{id}")]
        public async Task<IActionResult> RemoveEducation(long id)
        {
            var education = await DB.Educations.SingleAsync(x => x.Id == id);
            DB.Educations.Remove(education);
            await DB.SaveChangesAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Education experiencehas been removed successfully."];
                x.RedirectText = SR["Back To Education List"];
                x.RedirectUrl = Url.Action("Education", "Admin");
            });
        }

        [HttpGet]
        [AdminRequired]
        [Route("[controller]/Education/Edit/{id}")]
        public async Task<IActionResult> EditEducation(long id) => View(await DB.Educations.SingleAsync(x => x.Id == id));
        
        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Education/Edit/{id}")]
        public async Task<IActionResult> EditEducation(long id, Education Model)
        {
            var education = await DB.Educations.SingleAsync(x => x.Id == id);
            education.From = Model.From;
            education.Profession = Model.Profession;
            education.School = Model.School;
            education.To = Model.To;
            await DB.SaveChangesAsync();
            return Prompt(x => 
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Education experience has been saved successfully."];
                x.HideBack = true;
                x.RedirectText = SR["Back To Education List"];
                x.RedirectUrl = Url.Action("Education", "Admin");
            });
        }
        #endregion

        #region Skill Management
        [HttpGet]
        [AdminRequired]
        public async Task<IActionResult> Skill() => View(await DB.Skills.OrderBy(x => x.Performance).ThenByDescending(x => x.Level).ToListAsync());

        [HttpGet]
        [AdminRequired]
        [Route("[controller]/Skill/Create")]
        public IActionResult CreateSkill() => View();

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
                x.HideBack = true;
                x.RedirectText = SR["Back To Skill List"];
                x.RedirectUrl = Url.Action("Skill", "Admin");
            });
        }


        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Skill/Remove/{id}")]
        public async Task<IActionResult> RemoveSkill(long id)
        {
            var skill = await DB.Skills.SingleAsync(x => x.Id == id);
            DB.Skills.Remove(skill);
            await DB.SaveChangesAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Skill has been removed successfully."];
                x.HideBack = true;
                x.RedirectText = SR["Back To Skill List"];
                x.RedirectUrl = Url.Action("Skill", "Admin");
            });
        }

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Skill/Edit/{id}")]
        public async Task<IActionResult> EditSkill(long id, int level)
        {
            var skill = await DB.Skills.SingleAsync(x => x.Id == id);
            skill.Level = level;
            await DB.SaveChangesAsync();
            return Prompt(x => 
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Skill has been saved successfully."];
                x.HideBack = true;
                x.RedirectText = SR["Back To Skill List"];
                x.RedirectUrl = Url.Action("Skill", "Admin");
            });
        }
        #endregion

        #region Certificate Management
        [HttpGet]
        [AdminRequired]
        public async Task<IActionResult> Certificate() => View(await DB.Certificates.OrderByDescending(x => x.PRI).ToListAsync());

        [HttpGet]
        [AdminRequired]
        [Route("[controller]/Certificate/Create")]
        public IActionResult CreateCertificate() => View();

        [HttpGet]
        [AdminRequired]
        [Route("[controller]/Certificate/Edit/{id}")]
        public async Task<IActionResult> EditCertificate(long id) => View(await DB.Certificates.SingleAsync(x => x.Id == id));

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Certificate/Edit/{id}")]
        public async Task<IActionResult> EditCertificate(long id, Certificate Model, IFormFile File)
        {
            var certificate = await DB.Certificates.SingleAsync(x => x.Id == id);
            certificate.PRI = Model.PRI;
            certificate.Title = Model.Title;
            if (File != null && File.Length > 0)
            {
                var file = new Pomelo.AspNetCore.Extensions.BlobStorage.Models.Blob
                {
                    Bytes = await File.ReadAllBytesAsync(),
                    ContentLength = File.Length,
                    FileName = File.FileName,
                    ContentType = File.ContentType,
                    Time = DateTime.Now
                };
                DB.Blobs.Add(file);
                await DB.SaveChangesAsync();
                certificate.BlobId = file.Id;
            }
            await DB.SaveChangesAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Certificate has been saved successfully."];
                x.HideBack = true;
                x.RedirectText = SR["Back To Certificate List"];
                x.RedirectUrl = Url.Action("Certificate", "Admin");
            });
        }

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Certificate/Create")]
        public async Task<IActionResult> CreateCertificate(Certificate Model, IFormFile File)
        {
            if (File != null && File.Length > 0)
            {
                var file = new Pomelo.AspNetCore.Extensions.BlobStorage.Models.Blob
                {
                    Bytes = await File.ReadAllBytesAsync(),
                    ContentLength = File.Length,
                    FileName = File.FileName,
                    ContentType = File.ContentType,
                    Time = DateTime.Now
                };
                DB.Blobs.Add(file);
                await DB.SaveChangesAsync();
                Model.BlobId = file.Id;
            }
            DB.Certificates.Add(Model);
            await DB.SaveChangesAsync();
            return Prompt(x => 
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Certificate has been created successfully."];
                x.HideBack = true;
                x.RedirectText = SR["Back To Certificate List"];
                x.RedirectUrl = Url.Action("Certificate", "Admin");
            });
        }

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Certificate/Remove/{id}")]
        public async Task<IActionResult> RemoveCertificate(long id)
        {
            var certificate = await DB.Certificates.SingleAsync(x => x.Id == id);
            DB.Certificates.Remove(certificate);
            await DB.SaveChangesAsync();
            return Prompt(x =>
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Certificate has been removed successfully."];
                x.HideBack = true;
                x.RedirectText = SR["Back To Certificate List"];
                x.RedirectUrl = Url.Action("Certificate", "Admin");
            });
        }
        #endregion

        #region Experience Management
        [HttpGet]
        [AdminRequired]
        public async Task<IActionResult> Experience() => View(await DB.Experiences.OrderByDescending(x => x.From).ThenBy(x => x.To).ToListAsync());

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Experience/Remove/{id}")]
        public async Task<IActionResult> RemoveExperience(long id)
        {
            var experience = await DB.Experiences.SingleAsync(x => x.Id == id);
            DB.Experiences.Remove(experience);
            await DB.SaveChangesAsync();
            return Prompt(x => 
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Working experience has been removed successfully."];
                x.HideBack = true;
                x.RedirectText = SR["Back To Working Experience List"];
                x.RedirectUrl = Url.Action("Experience", "Admin");
            });
        }

        [HttpGet]
        [AdminRequired]
        [Route("[controller]/Experience/Edit/{id}")]
        public async Task<IActionResult> EditExperience(long id) => View(await DB.Experiences.SingleAsync(x => x.Id == id));

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Experience/Edit/{id}")]
        public async Task<IActionResult> EditExperience(long id, Experience Model)
        {
            var experience = await DB.Experiences.SingleAsync(x => x.Id == id);
            experience.Company = Model.Company;
            experience.Description = Model.Description;
            experience.From = Model.From;
            experience.To = Model.To;
            experience.Position = Model.Position;
            await DB.SaveChangesAsync();
            return Prompt(x => 
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Working experience has been saved successfully."];
                x.HideBack = true;
                x.RedirectText = SR["Back To Working Experience List"];
                x.RedirectUrl = Url.Action("Experience", "Admin");
            });
        }

        [HttpGet]
        [AdminRequired]
        [Route("[controller]/Experience/Create")]
        public IActionResult CreateExperience() => View();

        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Experience/Create")]
        public async Task<IActionResult> CreateExperience(Experience Model)
        {
            DB.Experiences.Add(Model);
            await DB.SaveChangesAsync();
            return Prompt(x => 
            {
                x.Title = SR["Succeeded"];
                x.Details = SR["Working experience has been created successfully."];
                x.HideBack = true;
                x.RedirectText = SR["Back To Working Experience List"];
                x.RedirectUrl = Url.Action("Experience", "Admin");
            });
        }
        #endregion

        #region Log Management
        [HttpGet]
        [AdminRequired]
        public async Task<IActionResult> Visitor(DateTime? Begin, DateTime? End, bool ShowAll = false)
        {
            foreach (var x in DB.Logs.Where(x => !x.IsRead))
                x.IsRead = true;
            await DB.SaveChangesAsync();
            IEnumerable<Log> ret = DB.Logs;
            if (Begin.HasValue)
                ret = ret.Where(x => x.Time >= Begin.Value);
            if (End.HasValue)
                ret = ret.Where(x => x.Time <= End.Value);
            if (!ShowAll)
                ret = ret.Where(x => !x.IsRead);
            return PagedView(ret.OrderByDescending(x => x.Time));
        }
        #endregion
    }
}
