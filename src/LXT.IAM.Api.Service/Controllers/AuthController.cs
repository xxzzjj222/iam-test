using LXT.IAM.Api.Bll.Services.Auth;
using LXT.IAM.Api.Bll.Services.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login/password")]
    public async Task<LoginOutput> LoginByPasswordAsync([FromBody] LoginByPasswordInput input)
    {
        return await _authService.LoginByPasswordAsync(input);
    }

    [AllowAnonymous]
    [HttpPost("login/code")]
    public async Task<LoginOutput> LoginByCodeAsync([FromBody] LoginByCodeInput input)
    {
        return await _authService.LoginByCodeAsync(input);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<LoginOutput> RegisterAsync([FromBody] RegisterInput input)
    {
        return await _authService.RegisterAsync(input);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<LoginOutput> RefreshTokenAsync([FromBody] RefreshTokenInput input)
    {
        return await _authService.RefreshTokenAsync(input);
    }

    [HttpGet("me")]
    public async Task<CurrentUserOutput> GetCurrentUserAsync()
    {
        return await _authService.GetCurrentUserAsync();
    }
}
