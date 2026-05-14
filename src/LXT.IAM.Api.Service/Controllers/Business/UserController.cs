using LXT.IAM.Api.Bll.Services.User;
using LXT.IAM.Api.Bll.Services.User.Dtos;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

/// <summary>
/// 用户管理控制�?/// </summary>
[Route("api/user")]
public class UserController : BusinessController
{
    private readonly IUserService _userService;

    /// <summary>
    /// 构�?    /// </summary>
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// 分页查询用户
    /// </summary>
    [HttpPost("page")]
    public async Task<PagedList<UserOutput>> GetPagedListAsync([FromBody] GetUserPagedListInput input)
    {
        return await _userService.GetPagedListAsync(input);
    }

    /// <summary>
    /// 获取用户详情
    /// </summary>
    [HttpGet("{UserId}")]
    public async Task<UserOutput> GetAsync(Guid UserId)
    {
        return await _userService.GetAsync(UserId);
    }

    /// <summary>
    /// 冻结用户
    /// </summary>
    [HttpPut("{UserId}/freeze")]
    public async Task FreezeAsync(Guid UserId)
    {
        await _userService.FreezeAsync(UserId);
    }

    /// <summary>
    /// 解冻用户
    /// </summary>
    [HttpPut("{UserId}/unfreeze")]
    public async Task UnfreezeAsync(Guid UserId)
    {
        await _userService.UnfreezeAsync(UserId);
    }

    /// <summary>
    /// 分配用户应用访问权限
    /// </summary>
    [HttpPost("{UserId}/apps")]
    public async Task AssignAppsAsync(Guid UserId, [FromBody] AssignUserAppsInput input)
    {
        await _userService.AssignAppsAsync(UserId, input);
    }

    /// <summary>
    /// 重置用户密码
    /// </summary>
    [HttpPut("{UserId}/reset-password")]
    public async Task ResetPasswordAsync(Guid UserId, [FromBody] ResetPasswordInput input)
    {
        await _userService.ResetPasswordAsync(UserId, input);
    }
}

