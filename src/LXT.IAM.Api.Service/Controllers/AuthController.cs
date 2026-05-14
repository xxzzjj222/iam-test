using LXT.IAM.Api.Bll.Services.Auth;
using LXT.IAM.Api.Bll.Services.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers;

/// <summary>
/// 认证控制器
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// 构造
    /// </summary>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// 账号密码登录
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login/password")]
    public async Task<LoginOutput> LoginByPasswordAsync([FromBody] LoginByPasswordInput input)
    {
        return await _authService.LoginByPasswordAsync(input);
    }

    /// <summary>
    /// 验证码登录
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login/code")]
    public async Task<LoginOutput> LoginByCodeAsync([FromBody] LoginByCodeInput input)
    {
        return await _authService.LoginByCodeAsync(input);
    }

    /// <summary>
    /// 注册
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<LoginOutput> RegisterAsync([FromBody] RegisterInput input)
    {
        return await _authService.RegisterAsync(input);
    }

    /// <summary>
    /// 刷新令牌
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<LoginOutput> RefreshTokenAsync([FromBody] RefreshTokenInput input)
    {
        return await _authService.RefreshTokenAsync(input);
    }

    /// <summary>
    /// 获取当前用户信息
    /// </summary>
    [HttpGet("me")]
    public async Task<CurrentUserOutput> GetCurrentUserAsync()
    {
        return await _authService.GetCurrentUserAsync();
    }

    /// <summary>
    /// 修改当前用户密码
    /// </summary>
    [HttpPut("change-password")]
    public async Task ChangePasswordAsync([FromBody] ChangePasswordInput input)
    {
        await _authService.ChangePasswordAsync(input);
    }
}
