using Microsoft.AspNetCore.Identity;
using LibraryRepository.Models;
using LibraryServices.Interfaces;

public class RoleInitializer
{
    public static async Task InitializeAsync(IUserServices userServices,
        string adminEmail, string adminPassword)
    {
        if (await userServices.FindByNameAsync(adminEmail) == null)
        {
            User admin = new User
            {
                Email = adminEmail,
                UserName = adminEmail
            };
            
            IdentityResult result = await userServices.AddAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                await userServices.AddToRoleAsync(admin, "admin");
            }
        }
    }
}
