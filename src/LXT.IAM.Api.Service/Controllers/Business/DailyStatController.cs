using LXT.IAM.Api.Bll.Services.DailyStat;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

/// <summary>
/// 每日统计控制器
/// </summary>
[Route("api/daily-stat")]
public class DailyStatController : BusinessController
{
    private readonly IDailyStatService _dailyStatService;

    /// <summary>
    /// 构造
    /// </summary>
    public DailyStatController(IDailyStatService dailyStatService)
    {
        _dailyStatService = dailyStatService;
    }

    /// <summary>
    /// 刷新每日用户统计
    /// </summary>
    [HttpPost("refresh")]
    public async Task RefreshAsync([FromQuery] DateTime? statDate)
    {
        await _dailyStatService.RefreshDailyUserStatAsync(statDate);
    }
}
