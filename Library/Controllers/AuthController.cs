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
        return Ok(await _refreshTokens.GetAuthTokensAsync(model.Email, model.Password, cancellationToken));

    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {

        return Ok(await _refreshTokens.RefreshAccessTokenAsync(request.AccessToken, request.RefreshToken, cancellationToken));
    }
}