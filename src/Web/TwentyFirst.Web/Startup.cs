﻿namespace TwentyFirst.Web
{
    using AutoMapper;
    using Common.Mapping;
    using Common.Models.Articles;
    using Data;
    using Data.Models;
    using Infrastructure.Extensions;
    using Logging;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using reCAPTCHA.AspNetCore;
    using Services.AuthMessageSender;
    using Services.CloudFileUploader;
    using System;
    using Common.Constants;
    using Filters;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AutoMapperConfig.RegisterMappings(typeof(ArticleCreateInputModel).Assembly);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<TwentyFirstDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<TwentyFirstDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = GlobalConstants.MinPasswordLength;
                options.Password.RequiredUniqueChars = GlobalConstants.RequiredUniqueChars;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = GlobalConstants.AllowedUserNameCharacters;
                options.User.RequireUniqueEmail = false;

                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);

                options.LoginPath = GlobalConstants.AdministrationLoginPage;
                options.LogoutPath = GlobalConstants.AdministrationLogoutPage;
                options.AccessDeniedPath = GlobalConstants.AdministrationAccessDeniedPage;
                options.SlidingExpiration = true;
            });

            services.AddAutoMapper();

            services.AddDomainServices();

            services.AddMvc(options =>
            {
                options.Filters.Add<ErrorPageExceptionFilterAttribute>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<ICloudFileUploader, CloudinaryFileUploader>();
            services.Configure<CloudFileUploaderOptions>(Configuration.GetSection("CloudinaryAccount"));

            services.AddSingleton<IEmailSender, SendGridEmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration.GetSection("SendGridAccount"));

            services.AddTransient<IRecaptchaService, RecaptchaService>();
            services.Configure<RecaptchaSettings>(Configuration.GetSection("RecaptchaSettings"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UpdateDatabase();
            app.SeedDatabaseAsync();

            loggerFactory.AddContext(app, LogLevel.Error);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
