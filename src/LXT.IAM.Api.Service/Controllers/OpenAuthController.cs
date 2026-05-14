using LXT.IAM.Api.Bll.Services.OpenAuth;
using LXT.IAM.Api.Bll.Services.OpenAuth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers;

/// <summary>
/// 开放认证控制器
/// </summary>
[ApiController]
[Route("api/open-auth")]
public class OpenAuthController : ControllerBase
{
    private readonly IOpenAuthService _openAuthService;

    /// <summary>
    /// 构造
    /// </summary>
    public OpenAuthController(IOpenAuthService openAuthService)
    {
        _openAuthService = openAuthService;
    }

    /// <summary>
    /// 获取客户端凭证令牌
    /// </summary>
    [AllowAnonymous]
    [HttpPost("token")]
    public async Task<ClientCredentialTokenOutput> GetClientTokenAsync([FromBody] ClientCredentialTokenInput input)
    {
        return await _openAuthService.GetClientTokenAsync(input);
    }

    /// <summary>
    /// 检查令牌状态
    /// </summary>
    [AllowAnonymous]
    [HttpPost("introspect")]
    public async Task<IntrospectTokenOutput> IntrospectAsync([FromBody] IntrospectTokenInput input)
    {
        return await _openAuthService.IntrospectAsync(input);
    }
}
