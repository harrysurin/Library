using Microsoft.AspNetCore.Mvc;
using LibraryRepository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LibraryServices.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
   
    private readonly UserManager<User> _userManager;
    private readonly IUserServices _userServices;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokensService _refreshTokens;

    public AuthController(UserManager<User> userManager,
        IUserServices userServices,
        ITokenService tokenService,
        IRefreshTokensService refreshTokens
        )
    {
        _userManager = userManager;
        _userServices = userServices;
        _tokenService = tokenService;
        _refreshTokens = refreshTokens;
        
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterUser(string email, string password, string role)
    {
        if (await _userServices.RegisterUser(email, password, role))
        {
            return Ok("Successfully done");
        }
        return BadRequest("Something went wrong");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized();

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(_tokenService.GetRefreshTokenExpirationDays())
        };

        await _refreshTokens.AddAsync(refreshTokenEntity, cancellationToken);

        return Ok(new
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = _tokenService.GetAccessTokenExpirationMinutes() * 60
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var username = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
            return Unauthorized();

        var refreshToken = await _refreshTokens
            .GetTokenAsync(request.RefreshToken, user.Id, cancellationToken);

        if (refreshToken == null || refreshToken.IsExpired)
            return Unauthorized();

        
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        
        await _refreshTokens.Delete(refreshToken);
        
        RefreshToken newToken = new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(_tokenService.GetRefreshTokenExpirationDays())
        };
        await _refreshTokens.AddAsync(newToken, cancellationToken);

        return Ok(new
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }
}