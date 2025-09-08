using Microsoft.AspNetCore.Identity;
using star_events.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace star_events.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            SeedRoles(roleManager);
            SeedAdminUser(userManager);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Roles from your requirements
            string[] roleNames = { "Admin", "EventOrganizer", "Customer" };

            foreach (var roleName in roleNames)
            {
                var roleExist = roleManager.RoleExistsAsync(roleName).Result;
                if (!roleExist)
                {
                    roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
                }
            }
        }

        private static void SeedAdminUser(UserManager<ApplicationUser> userManager)
        {
            // Check if the admin user already exists
            if (userManager.FindByEmailAsync("admin@starevents.lk").Result == null)
            {
                ApplicationUser adminUser = new ApplicationUser
                {
                    UserName = "admin@starevents.lk",
                    Email = "admin@starevents.lk",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(adminUser, "Admin@123").Result;

                if (result.Succeeded)
                {
                    // Assign the new user to the "Admin" role
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }
            }
        }
    }
}

