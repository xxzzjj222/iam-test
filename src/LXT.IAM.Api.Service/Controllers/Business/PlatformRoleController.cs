using LXT.IAM.Api.Bll.Services.PlatformRole;
using LXT.IAM.Api.Bll.Services.PlatformRole.Dtos;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

/// <summary>
/// 平台角色控制器
/// </summary>
[Route("api/platform-role")]
public class PlatformRoleController : BusinessController
{
    private readonly IPlatformRoleService _platformRoleService;

    /// <summary>
    /// 构造
    /// </summary>
    public PlatformRoleController(IPlatformRoleService platformRoleService)
    {
        _platformRoleService = platformRoleService;
    }

    /// <summary>
    /// 分页查询平台角色
    /// </summary>
    [HttpPost("page")]
    public async Task<PagedList<PlatformRoleOutput>> GetPagedListAsync([FromBody] GetPlatformRolePagedListInput input)
    {
        return await _platformRoleService.GetPagedListAsync(input);
    }

    /// <summary>
    /// 获取平台角色详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<PlatformRoleOutput> GetAsync(long id)
    {
        return await _platformRoleService.GetAsync(id);
    }

    /// <summary>
    /// 获取平台角色下拉选项
    /// </summary>
    [HttpGet("options")]
    public async Task<List<LongStringKV>> GetOptionsAsync()
    {
        return await _platformRoleService.GetOptionsAsync();
    }

    /// <summary>
    /// 新增平台角色
    /// </summary>
    [HttpPost]
    public async Task<long> AddAsync([FromBody] AddPlatformRoleInput input)
    {
        return await _platformRoleService.AddAsync(input);
    }

    /// <summary>
    /// 修改平台角色
    /// </summary>
    [HttpPut("{id}")]
    public async Task PutAsync(long id, [FromBody] PutPlatformRoleInput input)
    {
        await _platformRoleService.PutAsync(id, input);
    }

    /// <summary>
    /// 删除平台角色
    /// </summary>
    [HttpDelete("{id}")]
    public async Task DeleteAsync(long id)
    {
        await _platformRoleService.DeleteAsync(id);
    }

    /// <summary>
    /// 为用户分配平台角色
    /// </summary>
    [HttpPost("assign-user-roles")]
    public async Task AssignUserRolesAsync([FromBody] AssignUserPlatformRolesInput input)
    {
        await _platformRoleService.AssignUserRolesAsync(input);
    }
}
