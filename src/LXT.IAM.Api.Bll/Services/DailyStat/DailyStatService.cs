using LXT.IAM.Api.Storage.Context;
using LXT.IAM.Api.Storage.Entity;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Bll.Services.DailyStat;

public class DailyStatService : IDailyStatService
{
    private readonly IAMDbContext _db;

    public DailyStatService(IAMDbContext db)
    {
        _db = db;
    }

    public async Task RefreshDailyUserStatAsync(DateTime? statDate = null)
    {
        var date = (statDate ?? DateTime.UtcNow.Date).Date;
        var appCodes = await _db.App.Select(x => x.Code).ToListAsync();

        var oldStats = await _db.DailyUserStat.Where(x => x.StatDate == date).ToListAsync();
        _db.DailyUserStat.RemoveRange(oldStats);

        foreach (var appCode in appCodes)
        {
            var totalUserCount = await (from ua in _db.UserApp
                                        join app in _db.App on ua.AppId equals app.Id
                                        where app.Code == appCode
                                        select ua.CommonUserId).Distinct().CountAsync();

            var newUserCount = await _db.CommonUser.CountAsync(x => x.RegisterAppCode == appCode && x.CreateTime >= date && x.CreateTime < date.AddDays(1));

            var activeUserCount = await _db.UserAppActivityDaily
                .Where(x => x.AppCode == appCode && x.StatDate == date)
                .Select(x => x.CommonUserId)
                .Distinct()
                .CountAsync();

            _db.DailyUserStat.Add(new DailyUserStatEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                StatDate = date,
                AppCode = appCode,
                TotalUserCount = totalUserCount,
                NewUserCount = newUserCount,
                ActiveUserCount = activeUserCount
            });
        }

        await _db.SaveChangesAsync();
    }
}
