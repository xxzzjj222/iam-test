using LXT.IAM.Api.Bll.Services.PlatformFunction;
using LXT.IAM.Api.Bll.Services.PlatformFunction.Dtos;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

/// <summary>
/// 平台功能控制器
/// </summary>
[Route("api/platform-function")]
public class PlatformFunctionController : BusinessController
{
    private readonly IPlatformFunctionService _platformFunctionService;

    /// <summary>
    /// 构造
    /// </summary>
    public PlatformFunctionController(IPlatformFunctionService platformFunctionService)
    {
        _platformFunctionService = platformFunctionService;
    }

    /// <summary>
    /// 获取平台功能树
    /// </summary>
    [HttpGet("tree")]
    public async Task<List<PlatformFunctionOutput>> GetTreeAsync()
    {
        return await _platformFunctionService.GetTreeAsync();
    }

    /// <summary>
    /// 获取当前用户可见功能树
    /// </summary>
    [HttpGet("current-user-functions")]
    public async Task<List<PlatformFunctionOutput>> GetCurrentUserFunctionsAsync()
    {
        return await _platformFunctionService.GetCurrentUserFunctionsAsync();
    }

    /// <summary>
    /// 获取平台功能详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<PlatformFunctionOutput> GetAsync(long id)
    {
        return await _platformFunctionService.GetAsync(id);
    }

    /// <summary>
    /// 新增平台功能
    /// </summary>
    [HttpPost]
    public async Task<long> AddAsync([FromBody] AddPlatformFunctionInput input)
    {
        return await _platformFunctionService.AddAsync(input);
    }

    /// <summary>
    /// 修改平台功能
    /// </summary>
    [HttpPut("{id}")]
    public async Task PutAsync(long id, [FromBody] PutPlatformFunctionInput input)
    {
        await _platformFunctionService.PutAsync(id, input);
    }

    /// <summary>
    /// 删除平台功能
    /// </summary>
    [HttpDelete("{id}")]
    public async Task DeleteAsync(long id)
    {
        await _platformFunctionService.DeleteAsync(id);
    }

    /// <summary>
    /// 为角色分配平台功能
    /// </summary>
    [HttpPost("assign-role-functions")]
    public async Task AssignRoleFunctionsAsync([FromBody] AssignPlatformRoleFunctionsInput input)
    {
        await _platformFunctionService.AssignRoleFunctionsAsync(input);
    }
}
