using LXT.IAM.Api.Bll.Services.User;
using LXT.IAM.Api.Bll.Services.User.Dtos;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

/// <summary>
/// 用户管理控制器
/// </summary>
[Route("api/user")]
public class UserController : BusinessController
{
    private readonly IUserService _userService;

    /// <summary>
    /// 构造
    /// </summary>
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
    [HttpGet("{commonUserId}")]
    public async Task<UserOutput> GetAsync(Guid commonUserId)
    {
        return await _userService.GetAsync(commonUserId);
    }

    /// <summary>
    /// 冻结用户
    /// </summary>
    [HttpPut("{commonUserId}/freeze")]
    public async Task FreezeAsync(Guid commonUserId)
    {
        await _userService.FreezeAsync(commonUserId);
    }

    /// <summary>
    /// 解冻用户
    /// </summary>
    [HttpPut("{commonUserId}/unfreeze")]
    public async Task UnfreezeAsync(Guid commonUserId)
    {
        await _userService.UnfreezeAsync(commonUserId);
    }

    /// <summary>
    /// 分配用户应用访问权限
    /// </summary>
    [HttpPost("{commonUserId}/apps")]
    public async Task AssignAppsAsync(Guid commonUserId, [FromBody] AssignUserAppsInput input)
    {
        await _userService.AssignAppsAsync(commonUserId, input);
    }

    /// <summary>
    /// 重置用户密码
    /// </summary>
    [HttpPut("{commonUserId}/reset-password")]
    public async Task ResetPasswordAsync(Guid commonUserId, [FromBody] ResetPasswordInput input)
    {
        await _userService.ResetPasswordAsync(commonUserId, input);
    }
}
