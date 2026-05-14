using LXT.IAM.Api.Bll.Services.SystemInit;
using LXT.IAM.Api.Bll.Services.SystemInit.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers;

/// <summary>
/// 系统初始化控制器
/// </summary>
[ApiController]
[Route("api/system-init")]
public class SystemInitController : ControllerBase
{
    private readonly ISystemInitService _systemInitService;

    /// <summary>
    /// 构造
    /// </summary>
    public SystemInitController(ISystemInitService systemInitService)
    {
        _systemInitService = systemInitService;
    }

    /// <summary>
    /// 初始化系统
    /// </summary>
    [AllowAnonymous]
    [HttpPost("initialize")]
    public async Task<InitializeSystemOutput> InitializeAsync([FromBody] InitializeSystemInput input)
    {
        return await _systemInitService.InitializeAsync(input);
    }

    /// <summary>
    /// 查询初始化状态
    /// </summary>
    [AllowAnonymous]
    [HttpGet("status")]
    public async Task<SystemInitStatusOutput> GetStatusAsync()
    {
        return await _systemInitService.GetStatusAsync();
    }

    /// <summary>
    /// 修复系统初始化数据
    /// </summary>
    [AllowAnonymous]
    [HttpPost("repair")]
    public async Task<SystemRepairOutput> RepairAsync()
    {
        return await _systemInitService.RepairAsync();
    }
}
