using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Pomelo.AspNetCore.Localization;
using YuukoResume.Models;

namespace YuukoResume
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddViewLocalization();

            services.AddEntityFrameworkSqlite()
                .AddDbContext<ResumeContext>(x => x.UseSqlite("resume.db"));

            services.AddBlobStorage()
                .AddEntityFrameworkStorage<ResumeContext>();

            services.AddMemoryCache();

            services.AddSession(x => x.IdleTimeout = new TimeSpan(1, 0, 0));

            services.AddSmartCookies();

            services.AddPomeloLocalization(x =>
            {
                x.AddCulture(new[] { "zh", "zh-CN", "zh-Hans" }, new JsonLocalizedStringStore(Path.Combine("Localization", "zh-CN.json")));
                x.AddCulture(new[] { "en", "en-US" }, new JsonLocalizedStringStore(Path.Combine("Localization", "en-US.json")));
            })
                .AddBaiduTranslator();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseStaticFiles();
            app.UseSession();
            app.UseFrontendLocalizer();
            app.UseDeveloperExceptionPage();
            app.UseMvcWithDefaultRoute();
        }
    }
}
