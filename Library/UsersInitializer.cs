using Microsoft.AspNetCore.Identity;
using LibraryRepository.Models;
using LibraryServices.Interfaces;

public class UsersInitializer
{
    public static async Task InitializeAsync(IUserServices userServices,
        string adminEmail, string adminPassword, CancellationToken token = default)
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
                var savedUser = await userServices.FindByEmailAsync(adminEmail);
                if (savedUser == null)
                {
                    Console.WriteLine("Admin user not found, cannot assign admin role to the user");
                    return;
                }

                var roleAssignmentResult = await userServices.AddToRoleAsync(savedUser, "Admin");

                if (!roleAssignmentResult.Succeeded)
                {
                    Console.WriteLine("Cannot assign admin role:");
                    Console.WriteLine(roleAssignmentResult.Errors);
                }
            }
        }
    }
}
