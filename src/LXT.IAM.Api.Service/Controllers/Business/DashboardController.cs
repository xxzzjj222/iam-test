using LXT.IAM.Api.Bll.Services.Dashboard;
using LXT.IAM.Api.Bll.Services.Dashboard.Dtos;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

[Route("api/dashboard")]
public class DashboardController : BusinessController
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("overview")]
    public async Task<DashboardOverviewOutput> GetOverviewAsync()
    {
        return await _dashboardService.GetOverviewAsync();
    }

    [HttpPost("user-trend")]
    public async Task<List<TrendPointOutput>> GetUserTrendAsync([FromBody] GetTrendInput input)
    {
        return await _dashboardService.GetUserTrendAsync(input);
    }

    [HttpPost("activity-trend")]
    public async Task<List<TrendPointOutput>> GetActivityTrendAsync([FromBody] GetTrendInput input)
    {
        return await _dashboardService.GetActivityTrendAsync(input);
    }

    [HttpPost("business-trend")]
    public async Task<List<TrendPointOutput>> GetBusinessTrendAsync([FromBody] GetTrendInput input)
    {
        return await _dashboardService.GetBusinessTrendAsync(input);
    }
}
