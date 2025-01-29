using LibraryRepository.Models;
using LibraryServices.Interfaces;
using  Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserServices : IUserServices
{
    private UserManager<User> userManager;

    public UserServices(UserManager<User> _userManager)
    {
        userManager = _userManager;
    }

    public async Task<bool> RegisterUser(string email, string password, string role)
    {
        var user = new User
        {
            UserName = email,
            Email = email
        };

        await userManager.AddToRoleAsync(user, role);

        var result = await userManager.CreateAsync(user, password);
        return result.Succeeded;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken) 
        => await userManager.Users.ToListAsync(cancellationToken);

    public async Task<IdentityResult> AddAsync(User user, string password) => await userManager.CreateAsync(user, password);

    public async Task DeleteAsync(User user) => await userManager.DeleteAsync(user);

    public async Task UpdateAsync(User user) => await userManager.UpdateAsync(user);

    public async Task<User?> FindByIdAsync(Guid userId) => await userManager.FindByIdAsync(userId.ToString());

    public async Task<User?> FindByNameAsync(string userName) => await userManager.FindByNameAsync(userName);

    public async Task<User?> FindByEmailAsync(string email) => await userManager.FindByEmailAsync(email);

    public async Task ChangePassword(User user, string oldPassword, string newPassword)
            => await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

    public async Task<bool> IsInRoleAsync(User user, string rolename) 
            => await userManager.IsInRoleAsync(user, rolename);
    
    public async Task<IdentityResult> AddToRoleAsync(User user, string rolename) 
            => await userManager.AddToRoleAsync(user, rolename);
}