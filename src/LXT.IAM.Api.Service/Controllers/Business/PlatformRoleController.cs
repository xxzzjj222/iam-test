using LXT.IAM.Api.Bll.Services.PlatformRole;
using LXT.IAM.Api.Bll.Services.PlatformRole.Dtos;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

[Route("api/platform-role")]
public class PlatformRoleController : BusinessController
{
    private readonly IPlatformRoleService _platformRoleService;

    public PlatformRoleController(IPlatformRoleService platformRoleService)
    {
        _platformRoleService = platformRoleService;
    }

    [HttpPost("page")]
    public async Task<PagedList<PlatformRoleOutput>> GetPagedListAsync([FromBody] GetPlatformRolePagedListInput input)
    {
        return await _platformRoleService.GetPagedListAsync(input);
    }

    [HttpGet("{id}")]
    public async Task<PlatformRoleOutput> GetAsync(long id)
    {
        return await _platformRoleService.GetAsync(id);
    }

    [HttpGet("options")]
    public async Task<List<LongStringKV>> GetOptionsAsync()
    {
        return await _platformRoleService.GetOptionsAsync();
    }

    [HttpPost]
    public async Task<long> AddAsync([FromBody] AddPlatformRoleInput input)
    {
        return await _platformRoleService.AddAsync(input);
    }

    [HttpPut("{id}")]
    public async Task PutAsync(long id, [FromBody] PutPlatformRoleInput input)
    {
        await _platformRoleService.PutAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(long id)
    {
        await _platformRoleService.DeleteAsync(id);
    }

    [HttpPost("assign-user-roles")]
    public async Task AssignUserRolesAsync([FromBody] AssignUserPlatformRolesInput input)
    {
        await _platformRoleService.AssignUserRolesAsync(input);
    }
}
