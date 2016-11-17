using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Pomelo.AspNetCore.Localization;
using Newtonsoft.Json;
using YuukoResume.Models;

namespace YuukoResume
{
    public class Startup
    {
        public static Profile Profile = JsonConvert.DeserializeObject<Profile>(File.ReadAllText("profile.json"));

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite()
                .AddDbContext<ResumeContext>(x => 
                {
                    x.UseSqlite("Data source=resume.db");
                });

            services.AddBlobStorage()
                .AddEntityFrameworkStorage<ResumeContext>();

            services.AddMemoryCache();

            services.AddSession(x => x.IdleTimeout = new TimeSpan(1, 0, 0));

            services.AddSmartCookies();

            services.AddMvc();

            services.AddPomeloLocalization(x =>
            {
                var cultures = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(Path.Combine("Localization", "cultures.json")));
                foreach (dynamic c in cultures)
                    x.AddCulture(c.Cultures.ToObject<string[]>(), new JsonLocalizedStringStore(Path.Combine("Localization", c.Source.ToString())));
            });

            services.AddSmtpEmailSender(Profile.SmtpServer, Profile.SmtpPort, Profile.Name.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Value)).Value, Profile.SmtpUsername, Profile.SmtpUsername, Profile.SmtpPassword);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseBlobStorage("/admin/js/jquery.pomelo.fileupload.js");
            app.UseStaticFiles();
            app.UseSession();
            app.UseDeveloperExceptionPage();
            app.UseMvcWithDefaultRoute();
            app.UseFrontendLocalizer();
            app.ApplicationServices.GetRequiredService<ResumeContext>().Database.Migrate();
        }
    }
}
