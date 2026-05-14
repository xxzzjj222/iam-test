using LXT.IAM.Api.Bll.Services.SystemInit;
using LXT.IAM.Api.Bll.Services.SystemInit.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers;

[ApiController]
[Route("api/system-init")]
public class SystemInitController : ControllerBase
{
    private readonly ISystemInitService _systemInitService;

    public SystemInitController(ISystemInitService systemInitService)
    {
        _systemInitService = systemInitService;
    }

    [AllowAnonymous]
    [HttpPost("initialize")]
    public async Task<InitializeSystemOutput> InitializeAsync([FromBody] InitializeSystemInput input)
    {
        return await _systemInitService.InitializeAsync(input);
    }

    [AllowAnonymous]
    [HttpGet("status")]
    public async Task<SystemInitStatusOutput> GetStatusAsync()
    {
        return await _systemInitService.GetStatusAsync();
    }

    [AllowAnonymous]
    [HttpPost("repair")]
    public async Task<SystemRepairOutput> RepairAsync()
    {
        return await _systemInitService.RepairAsync();
    }
}
