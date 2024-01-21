using Jiwebapi.Catalog.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Jiwebapi.Catalog.Identity.Seed
{
    public static class UserCreator
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var applicationUser = new ApplicationUser
            {
                FirstName = "Andrii",
                LastName = "Tkach",
                UserName = "andrii",
                Email = "andrii@email.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(applicationUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(applicationUser, "F1veL!fe");
            }
        }
    }
}