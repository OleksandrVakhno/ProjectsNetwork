using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectsNetwork.Data;
using ProjectsNetwork.Models;
using System;
using ProjectsNetwork.DataAccess.Repositories;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Services.IServices;
using ProjectsNetwork.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using ProjectsNetwork.Utils;
using ProjectsNetwork.Services.IApplicationService;

namespace ProjectsNetwork
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
            //Connecting to Postgre using defultconnection string from appsettings.json
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>(/*options => options.SignIn.RequireConfirmedAccount = true*/).AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<ISkillRepository, SkillRepository>();

            services.AddScoped<IUserSkillRepository, UserSkillRepository>();
            services.AddScoped<IProjectSkillRepository, ProjectSkillRepository>();
            services.AddScoped<IInterestedInProjectRepository, InterestedInProjectRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddSingleton<IEmailSender, EmailSender>();


            services.AddScoped<ISkillsService, SkillsService>();
            services.AddScoped<IProjectsService, ProjectsService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            


            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAuthentication()
                .AddMicrosoftAccount(microsoftOptions =>
                 {
                     microsoftOptions.ClientId = Configuration["AppSecrets:Authentication:Microsoft:ClientId"];
                     microsoftOptions.ClientSecret = Configuration["AppSecrets:Authentication:Microsoft:ClientSecret"];
                 })
                  .AddGoogle(options =>
                   {
                       IConfigurationSection googleAuthNSection =
                           Configuration.GetSection("AppSecrets:Authentication:Google");

                       options.ClientId = googleAuthNSection["ClientId"];
                       options.ClientSecret = googleAuthNSection["ClientSecret"];
                   });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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
                    name: "default",
                    pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

        }
    }
}
