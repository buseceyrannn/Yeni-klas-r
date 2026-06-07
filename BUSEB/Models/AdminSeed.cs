using Microsoft.AspNetCore.Identity;

namespace BUSEB.Models
{
    public static class AdminSeed
    {
        public static async Task SeedAdmin(
            UserManager<IdentityUser> userManager)
        {
            string adminEmail = "admin@buseb.com";

            string adminPassword = "Admin123!";

            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, adminPassword);

                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}