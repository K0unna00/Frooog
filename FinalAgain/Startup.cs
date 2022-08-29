using FinalAgain.DAL;
using FinalAgain.Hubs;
using FinalAgain.Models;
using FinalAgain.Services;
using FinalAgain.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAgain
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSignalR();
            string connectionString = Configuration.GetConnectionString("default");
            services.AddDbContext<AppDBContext>(c => c.UseSqlServer(connectionString));
            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = true;
                opt.Lockout.MaxFailedAccessAttempts = 4;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(4);

            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDBContext>();
            services.AddScoped<LayoutService>();
            Constants.EmailAddress = Configuration["Gmail:MailAddress"];
            Constants.Password = Configuration["Gmail:Password"];
            services.ConfigureApplicationCookie(opt =>
                opt.LoginPath = "/UserAccount/login"
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
                    name: "manage",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<UserHub>("/userHub");
            });
        }
    }
}
