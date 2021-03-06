﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MincePieRateV2.Web.Data;
using MincePieRateV2.DAL.Data;
using MincePieRateV2.DAL.Repositories;
using MincePieRateV2.Models.Domain;
using MincePieRateV2.DAL.ActionFilters;
using MincePieRateV2.Web.Authorization.Constants;
using MincePieRateV2.DAL.Managers;
using AutoMapper;
using MincePieRateV2.ViewModels.Domain;

namespace MincePieRateV2.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<MincePieCreateViewModel, MincePie>();
                CreateMap<MincePieDetailsViewModel, MincePie>().ReverseMap();
                CreateMap<ReviewDetailsViewModel, Review>().ReverseMap();
                CreateMap<ReviewCreateEditViewModel, Review>().ReverseMap();
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(UserInitialiserActionFilter));
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services
                .AddTransient(typeof(IRepository<MincePie>), typeof(MincePieRepository))
                .AddTransient(typeof(IRepository<Review>), typeof(ReviewRepository));

            if (_environment.IsDevelopment())
            {
                services.AddTransient(typeof(IImageManager), typeof(FileSystemImageManager));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            CreateRoles(serviceProvider);
            CreateDefaultUsers(serviceProvider, env);
        }

        private void CreateRoles(IServiceProvider serviceProvider)
        {
            //https://stackoverflow.com/q/42471866
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roleNames = new List<string>()
            {
                RoleConstants.UserRoleName,
                RoleConstants.AdministratorRoleName,
            };

            foreach (var roleName in roleNames)
            {
                var hasRole = roleManager.RoleExistsAsync(roleName);
                hasRole.Wait();

                if (!hasRole.Result)
                {
                    var roleResult = roleManager.CreateAsync(new IdentityRole(roleName));
                    roleResult.Wait();
                }
            }
        }

        private void CreateDefaultUsers(IServiceProvider serviceProvider, IWebHostEnvironment environment)
        {
            //https://stackoverflow.com/q/42471866
            if (environment.IsDevelopment())
            {
                //This account is only for development and should not be used in production!
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                string email = "development_administrator@mincepierate.com";
                string password = "DeleteThisAccountInProduction!1";
                var testUser = userManager.FindByEmailAsync(email);
                testUser.Wait();

                if (testUser.Result == null)
                {
                    var administrator = new IdentityUser
                    {
                        Email = email,
                        UserName = email
                    };

                    var newUser = userManager.CreateAsync(administrator, password);
                    newUser.Wait();

                    if (newUser.Result.Succeeded)
                    {
                        var newUserRole = userManager.AddToRoleAsync(administrator, RoleConstants.AdministratorRoleName);
                        newUserRole.Wait();
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
}
