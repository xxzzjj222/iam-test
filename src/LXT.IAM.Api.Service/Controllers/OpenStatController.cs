using LXT.IAM.Api.Bll.Services.OpenStat;
using LXT.IAM.Api.Bll.Services.OpenStat.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Service.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers;

/// <summary>
/// 开放统计控制器
/// </summary>
[ApiController]
[Authorize]
[Route("api/open-stat")]
public class OpenStatController : ControllerBase
{
    private readonly IOpenStatService _openStatService;

    /// <summary>
    /// 构造
    /// </summary>
    public OpenStatController(IOpenStatService openStatService)
    {
        _openStatService = openStatService;
    }

    /// <summary>
    /// 上报用户活跃
    /// </summary>
    [HttpPost("user-activity/report")]
    [RequireScope(PlatformConst.ScopeStatWrite)]
    public async Task ReportUserActivityAsync([FromBody] ReportUserActivityInput input)
    {
        await _openStatService.ReportUserActivityAsync(input);
    }

    /// <summary>
    /// 上报业务指标
    /// </summary>
    [HttpPost("business-metric/report")]
    [RequireScope(PlatformConst.ScopeStatWrite)]
    public async Task ReportBusinessMetricAsync([FromBody] ReportBusinessMetricInput input)
    {
        await _openStatService.ReportBusinessMetricAsync(input);
    }

    /// <summary>
    /// 上报角色快照
    /// </summary>
    [HttpPost("role-snapshot/report")]
    [RequireScope(PlatformConst.ScopeStatWrite)]
    public async Task ReportRoleSnapshotAsync([FromBody] ReportRoleSnapshotInput input)
    {
        await _openStatService.ReportRoleSnapshotAsync(input);
    }
}
