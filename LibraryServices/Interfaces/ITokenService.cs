using System.Security.Claims;
using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        int GetRefreshTokenExpirationDays();
        int GetAccessTokenExpirationMinutes();

    }
}