using LXT.IAM.Api.Bll.Services.User;
using LXT.IAM.Api.Bll.Services.User.Dtos;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

[Route("api/user")]
public class UserController : BusinessController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("page")]
    public async Task<PagedList<UserOutput>> GetPagedListAsync([FromBody] GetUserPagedListInput input)
    {
        return await _userService.GetPagedListAsync(input);
    }

    [HttpGet("{commonUserId}")]
    public async Task<UserOutput> GetAsync(Guid commonUserId)
    {
        return await _userService.GetAsync(commonUserId);
    }

    [HttpPut("{commonUserId}/freeze")]
    public async Task FreezeAsync(Guid commonUserId)
    {
        await _userService.FreezeAsync(commonUserId);
    }

    [HttpPut("{commonUserId}/unfreeze")]
    public async Task UnfreezeAsync(Guid commonUserId)
    {
        await _userService.UnfreezeAsync(commonUserId);
    }

    [HttpPost("{commonUserId}/apps")]
    public async Task AssignAppsAsync(Guid commonUserId, [FromBody] AssignUserAppsInput input)
    {
        await _userService.AssignAppsAsync(commonUserId, input);
    }
}
