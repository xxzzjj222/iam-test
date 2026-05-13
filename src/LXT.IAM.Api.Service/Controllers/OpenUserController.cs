using LXT.IAM.Api.Bll.Services.OpenUser;
using LXT.IAM.Api.Bll.Services.OpenUser.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Service.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers;

[ApiController]
[Authorize]
[Route("api/open-user")]
public class OpenUserController : ControllerBase
{
    private readonly IOpenUserService _openUserService;

    public OpenUserController(IOpenUserService openUserService)
    {
        _openUserService = openUserService;
    }

    [HttpGet("by-app")]
    [RequireScope(PlatformConst.ScopeUserRead)]
    public async Task<List<OpenUserOutput>> GetUsersByAppAsync([FromQuery] string appCode)
    {
        return await _openUserService.GetUsersByAppAsync(appCode);
    }

    [HttpGet("{commonUserId}")]
    [RequireScope(PlatformConst.ScopeUserRead)]
    public async Task<OpenUserOutput> GetByCommonUserIdAsync(Guid commonUserId)
    {
        return await _openUserService.GetByCommonUserIdAsync(commonUserId);
    }

    [HttpPost("batch")]
    [RequireScope(PlatformConst.ScopeUserRead)]
    public async Task<List<OpenUserOutput>> BatchGetAsync([FromBody] BatchOpenUserInput input)
    {
        return await _openUserService.BatchGetAsync(input);
    }
}
