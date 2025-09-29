using CampusConnect.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CampusConnect.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            if (!await roleManager.RoleExistsAsync("Student"))
            {
                await roleManager.CreateAsync(new IdentityRole("Student"));
            }

            if (!await roleManager.RoleExistsAsync("Faculty"))
            {
                await roleManager.CreateAsync(new IdentityRole("Faculty"));
            }
        }
    }
}
