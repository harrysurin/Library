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
    private readonly TokenService _tokenService;
    private readonly LibraryContext _context;

    public AuthController(UserManager<User> userManager,
        IUserServices userServices,
        TokenService tokenService,
        LibraryContext context)
    {
        _userManager = userManager;
        _userServices = userServices;
        _tokenService = tokenService;
        _context = context;
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
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
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

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = _tokenService.GetAccessTokenExpirationMinutes() * 60
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var username = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
            return Unauthorized();

        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken && rt.UserId == user.Id);

        if (refreshToken == null || refreshToken.IsExpired)
            return Unauthorized();

        
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        
        _context.RefreshTokens.Remove(refreshToken);
        
       
        _context.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(_tokenService.GetRefreshTokenExpirationDays())
        });
        await _context.SaveChangesAsync();

        return Ok(new
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }
}