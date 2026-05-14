using LXT.IAM.Api.Bll.Services.Dashboard;
using LXT.IAM.Api.Bll.Services.Dashboard.Dtos;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

/// <summary>
/// 仪表盘控制器
/// </summary>
[Route("api/dashboard")]
public class DashboardController : BusinessController
{
    private readonly IDashboardService _dashboardService;

    /// <summary>
    /// 构造
    /// </summary>
    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// 获取概览数据
    /// </summary>
    [HttpGet("overview")]
    public async Task<DashboardOverviewOutput> GetOverviewAsync()
    {
        return await _dashboardService.GetOverviewAsync();
    }

    /// <summary>
    /// 获取用户新增趋势
    /// </summary>
    [HttpPost("user-trend")]
    public async Task<List<TrendPointOutput>> GetUserTrendAsync([FromBody] GetTrendInput input)
    {
        return await _dashboardService.GetUserTrendAsync(input);
    }

    /// <summary>
    /// 获取活跃趋势
    /// </summary>
    [HttpPost("activity-trend")]
    public async Task<List<TrendPointOutput>> GetActivityTrendAsync([FromBody] GetTrendInput input)
    {
        return await _dashboardService.GetActivityTrendAsync(input);
    }

    /// <summary>
    /// 获取业务指标趋势
    /// </summary>
    [HttpPost("business-trend")]
    public async Task<List<TrendPointOutput>> GetBusinessTrendAsync([FromBody] GetTrendInput input)
    {
        return await _dashboardService.GetBusinessTrendAsync(input);
    }
}
