using System.Security.Claims;
using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IRefreshTokensService
    {
        Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task Delete(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetTokenAsync(string tokenId, Guid userId, CancellationToken cancellationToken = default);
        Task<AuthTokens> GetAuthTokensAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<AuthTokens> RefreshAccessTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken = default);
    }
}