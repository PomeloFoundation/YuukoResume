using System;
using System.IO;
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
            services.AddMvc()
                .AddViewLocalization();

            services.AddEntityFrameworkSqlite()
                .AddDbContext<ResumeContext>(x => 
                {
                    x.UseSqlite("Data source=resume.db");
                    x.UseSqliteLolita();
                });

            services.AddBlobStorage()
                .AddEntityFrameworkStorage<ResumeContext>();

            services.AddMemoryCache();

            services.AddSession(x => x.IdleTimeout = new TimeSpan(1, 0, 0));

            services.AddSmartCookies();

            services.AddPomeloLocalization(x =>
            {
                x.AddCulture(new[] { "zh", "zh-CN", "zh-Hans" }, new JsonLocalizedStringStore(Path.Combine("Localization", "zh-CN.json")));
                x.AddCulture(new[] { "en", "en-US" }, new JsonLocalizedStringStore(Path.Combine("Localization", "en-US.json")));
            });

            services.AddSmtpEmailSender("smtp.exmail.qq.com", 25, "Mano Cloud", "noreply@mano.cloud", "noreply@mano.cloud", "ManoCloud123456");
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseStaticFiles();
            app.UseSession();
            app.UseFrontendLocalizer();
            app.UseDeveloperExceptionPage();
            app.UseMvcWithDefaultRoute();
            app.ApplicationServices.GetRequiredService<ResumeContext>().Database.Migrate();
        }
    }
}
