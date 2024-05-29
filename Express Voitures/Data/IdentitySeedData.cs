using ExpressVoitures.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ExpressVoitures.Data
{
    public static class IdentitySeedData
    {
        private const string AdminEmail = "admin@expressvoitures.com";
        private const string AdminPassword = "Admin@12345";

        public static async Task EnsurePopulated(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Créer le rôle Admin s'il n'existe pas
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Créer l'utilisateur administrateur s'il n'existe pas
            var user = await userManager.FindByEmailAsync(AdminEmail);
            if (user == null)
            {
                user = new User { UserName = AdminEmail, Email = AdminEmail };
                var result = await userManager.CreateAsync(user, AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
