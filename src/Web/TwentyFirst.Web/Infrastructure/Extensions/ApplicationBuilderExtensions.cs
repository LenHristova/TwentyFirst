namespace TwentyFirst.Web.Infrastructure.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.Models.Enums;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UpdateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<TwentyFirstDbContext>();
                context.Database.Migrate();
            }

            return app;
        }

        public static async void SeedDatabaseAsync(this IApplicationBuilder app)
        {
            var serviceFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceFactory.CreateScope();
            using (scope)
            {
                await SeedRoles(scope);

                await SeedMasterAdministrator(scope);
            }
        }

        private static async Task SeedRoles(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = Enum.GetValues(typeof(Role));

            foreach (Role role in roles)
            {
                var roleName = role.GetDisplayName();

                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task SeedMasterAdministrator(IServiceScope scope)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var masterAdminUsername = configuration["MasterAdministratorAccount:Username"];
            var masterAdmin = await userManager.FindByNameAsync(masterAdminUsername);

            if (masterAdmin == null)
            {
                var masterAdminEmail = configuration["MasterAdministratorAccount:Email"];
                var masterAdminPassword = configuration["MasterAdministratorAccount:Password"];

                masterAdmin = new User
                {
                    UserName = masterAdminUsername,
                    Email = masterAdminEmail
                };

                await userManager.CreateAsync(masterAdmin, masterAdminPassword);
                await userManager.AddToRoleAsync(masterAdmin, Role.MasterAdmin.GetDisplayName());
            }
        }
    }
}
