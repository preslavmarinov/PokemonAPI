using Microsoft.AspNetCore.Identity;
using PokemonApi.Data.Models.Identity;

namespace PokemonApi.Web.Extensions
{
    public static class ApplicationUserExtension
    {
        public static async Task SeedRolesAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            if(roleManager.Roles.Any())
            {
                return;
            }

            var adminRole = CreateRole("admin");
            var userRole = CreateRole("user");

            await roleManager.CreateAsync(adminRole);
            await roleManager.CreateAsync(userRole);
        }

        public static async Task SeedUsersAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if(userManager.Users.Any())
            {
                return;
            }

            var admin = CreateUser("admin@gmail.com");

            var result =  await userManager.CreateAsync(admin, "admin12345");
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "admin");
            }
            else
            {
                throw new InvalidOperationException("User creation failed.");
            }
        }

        private static ApplicationRole CreateRole(string roleName)
        {
            return new ApplicationRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
            };
        }

        private static ApplicationUser CreateUser(string email)
        {
            return new ApplicationUser
            {
                UserName = email,
                NormalizedUserName = email.ToUpper(),
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
        }
    }
}
