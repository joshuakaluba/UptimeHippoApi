using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Models.Static;

namespace UptimeHippoApi.Data.DataContext
{
    public static class DataContextInitializer
    {
        public static Claim DefaultUserClaim => new Claim("DefaultUserClaim", "DefaultUserAuthorization");
        public static string UserRole = "User";

        public static async Task UpdateContext(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<UptimeHippoDataContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    await context.Database.MigrateAsync();
                }
            }
        }

        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roleCheck = await roleManager.RoleExistsAsync(UserRole);
            if (!roleCheck)
            {
                IdentityRole userRole = new IdentityRole(UserRole);
                await roleManager.CreateAsync(userRole);

                var testUser = await userManager.FindByEmailAsync(ApplicationConfig.TestUserEmail);

                if (testUser == null)
                {
                    var user = new ApplicationUser { UserName = ApplicationConfig.TestUserEmail, Email = ApplicationConfig.TestUserEmail };
                    userManager.CreateAsync(user, ApplicationConfig.TestUserPassword).Wait();

                    await userManager.AddToRoleAsync(user, UserRole);
                }
            }
        }
    }
}