using LXT.IAM.Api.Bll.Services.DailyStat;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

[Route("api/daily-stat")]
public class DailyStatController : BusinessController
{
    private readonly IDailyStatService _dailyStatService;

    public DailyStatController(IDailyStatService dailyStatService)
    {
        _dailyStatService = dailyStatService;
    }

    [HttpPost("refresh")]
    public async Task RefreshAsync([FromQuery] DateTime? statDate)
    {
        await _dailyStatService.RefreshDailyUserStatAsync(statDate);
    }
}
