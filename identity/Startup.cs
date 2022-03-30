using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using identity._Context;
using identity.Models;
using identity.Reposetory;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersianTranslation.Identity;

namespace identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            var myconnection = Configuration.GetConnectionString("Ef");
            services.AddDbContext<MyContext>(x => x.UseSqlServer(myconnection));
            services.AddAuthentication().AddGoogle(
                x =>{
                    x.ClientId = "72574947272-0gihsr0uupplb2lriet5i989384bq5bk.apps.googleusercontent.com";
                    x.ClientSecret = "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";
                });

        services.AddIdentity<IdentityUser, IdentityRole>(x =>
                {
                    x.Password.RequiredUniqueChars = 0;
                    x.User.RequireUniqueEmail=true;
                    x.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
                })
                .AddEntityFrameworkStores<MyContext>().AddDefaultTokenProviders()
                .AddErrorDescriber<PersianIdentityErrorDescriber>();
            services.AddScoped<IMessageSender, MessageSender>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
