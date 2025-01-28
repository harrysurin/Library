using LibraryRepository.Models;
using  Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace LibraryServices.Interfaces
{
    public interface IUserServices
    {
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IdentityResult> AddAsync(User user, string password);
        Task DeleteAsync(User user);
        Task UpdateAsync(User user);
        Task<User?> FindByIdAsync(Guid userId);
        Task<User?> FindByNameAsync(string userName);
        Task<User?> FindByEmailAsync(string email);
        Task ChangePassword(User user, string oldPassword, string newPassword);
        Task<bool> IsInRoleAsync(User user, string rolename);
        Task<IdentityResult> AddToRoleAsync(User user, string rolename);
    }
}