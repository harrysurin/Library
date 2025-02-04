using LibraryRepository.Models;
using LibraryServices.Interfaces;
using LibraryRepository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;

public class RefreshTokensServices : IRefreshTokensService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;

    public RefreshTokensServices(IUnitOfWork _unitOfWork, ITokenService tokenService, 
                                UserManager<User> userManager)
    {
        unitOfWork = _unitOfWork;
        _tokenService = tokenService;
        _userManager = userManager;

    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        unitOfWork.RefreshTokens.Add(refreshToken);
        await unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task Delete(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        unitOfWork.RefreshTokens.Delete(refreshToken);
        await unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<RefreshToken?> GetTokenAsync(string tokenId, Guid userId, CancellationToken cancellationToken)
    {
        return await unitOfWork.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == tokenId && rt.UserId == userId, cancellationToken);
    }

    public async Task<AuthTokens> GetAuthTokensAsync(string email, string password, CancellationToken token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new AuthenticationException();
            }

        var accessToken = await _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(_tokenService.GetRefreshTokenExpirationDays())
        };

        unitOfWork.RefreshTokens.Add(refreshTokenEntity);
        await unitOfWork.CompleteAsync(token);


        AuthTokens authToken = new AuthTokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = _tokenService.GetAccessTokenExpirationMinutes() * 60
        };
        return authToken;
    }

    public async Task<AuthTokens> RefreshAccessTokenAsync(string accessToken, string refreshToken, CancellationToken token)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
             {
                throw new AuthenticationException();
            }

        var refToken = await this
            .GetTokenAsync(refreshToken, user.Id, token);
            

        if (refToken == null || refToken.IsExpired)
            {
                throw new AuthenticationException();
            }

        
        var newAccessToken = await  _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        
        await this.Delete((RefreshToken)refToken, token);
        
        RefreshToken newToken = new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(_tokenService.GetRefreshTokenExpirationDays())
        };
        await this.AddAsync(newToken, token);

        AuthTokens authToken = new AuthTokens
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresIn = _tokenService.GetAccessTokenExpirationMinutes() * 60
        };
        return authToken;
    }
}