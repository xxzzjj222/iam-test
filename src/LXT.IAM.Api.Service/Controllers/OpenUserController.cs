using LXT.IAM.Api.Bll.Services.OpenUser;
using LXT.IAM.Api.Bll.Services.OpenUser.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Service.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers;

/// <summary>
/// 开放用户控制器
/// </summary>
[ApiController]
[Authorize]
[Route("api/open-user")]
public class OpenUserController : ControllerBase
{
    private readonly IOpenUserService _openUserService;

    /// <summary>
    /// 构�?    /// </summary>
    public OpenUserController(IOpenUserService openUserService)
    {
        _openUserService = openUserService;
    }

    /// <summary>
    /// 按应用查询用�?    /// </summary>
    [HttpGet("by-app")]
    [RequireScope(PlatformConst.ScopeUserRead)]
    public async Task<List<OpenUserOutput>> GetUsersByAppAsync([FromQuery] string appCode)
    {
        return await _openUserService.GetUsersByAppAsync(appCode);
    }

    /// <summary>
    /// 根据统一用户编号查询用户
    /// </summary>
    [HttpGet("{UserId}")]
    [RequireScope(PlatformConst.ScopeUserRead)]
    public async Task<OpenUserOutput> GetByUserIdAsync(Guid UserId)
    {
        return await _openUserService.GetByUserIdAsync(UserId);
    }

    /// <summary>
    /// 批量查询用户
    /// </summary>
    [HttpPost("batch")]
    [RequireScope(PlatformConst.ScopeUserRead)]
    public async Task<List<OpenUserOutput>> BatchGetAsync([FromBody] BatchOpenUserInput input)
    {
        return await _openUserService.BatchGetAsync(input);
    }
}

