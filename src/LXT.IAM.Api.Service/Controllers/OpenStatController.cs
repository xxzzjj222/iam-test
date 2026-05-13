using LXT.IAM.Api.Bll.Services.OpenStat;
using LXT.IAM.Api.Bll.Services.OpenStat.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Service.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers;

[ApiController]
[Authorize]
[Route("api/open-stat")]
public class OpenStatController : ControllerBase
{
    private readonly IOpenStatService _openStatService;

    public OpenStatController(IOpenStatService openStatService)
    {
        _openStatService = openStatService;
    }

    [HttpPost("user-activity/report")]
    [RequireScope(PlatformConst.ScopeStatWrite)]
    public async Task ReportUserActivityAsync([FromBody] ReportUserActivityInput input)
    {
        await _openStatService.ReportUserActivityAsync(input);
    }

    [HttpPost("business-metric/report")]
    [RequireScope(PlatformConst.ScopeStatWrite)]
    public async Task ReportBusinessMetricAsync([FromBody] ReportBusinessMetricInput input)
    {
        await _openStatService.ReportBusinessMetricAsync(input);
    }

    [HttpPost("role-snapshot/report")]
    [RequireScope(PlatformConst.ScopeStatWrite)]
    public async Task ReportRoleSnapshotAsync([FromBody] ReportRoleSnapshotInput input)
    {
        await _openStatService.ReportRoleSnapshotAsync(input);
    }
}
