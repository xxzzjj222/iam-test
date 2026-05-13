using LXT.IAM.Api.Bll.Services.Dashboard.Dtos;
using LXT.IAM.Api.Storage.Context;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Bll.Services.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly IAMDbContext _db;

    public DashboardService(IAMDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardOverviewOutput> GetOverviewAsync()
    {
        var totalUserCount = await _db.CommonUser.CountAsync();
        var today = DateTime.UtcNow.Date;
        var newUserCount = await _db.CommonUser.CountAsync(x => x.CreateTime >= today);
        var activeUserCount = await _db.UserAppActivityDaily.Where(x => x.StatDate == today).Select(x => x.CommonUserId).Distinct().CountAsync();
        var metrics = await _db.DailyBusinessStat
            .Where(x => x.StatDate == today)
            .Select(x => new AppMetricOutput
            {
                AppCode = x.AppCode,
                MetricCode = x.MetricCode,
                MetricName = x.MetricName,
                MetricValue = x.MetricValue
            }).ToListAsync();

        return new DashboardOverviewOutput
        {
            TotalUserCount = totalUserCount,
            NewUserCount = newUserCount,
            ActiveUserCount = activeUserCount,
            Metrics = metrics
        };
    }

    public async Task<List<TrendPointOutput>> GetUserTrendAsync(GetTrendInput input)
    {
        var (startDate, endDate) = NormalizeDateRange(input);
        var query = _db.DailyUserStat.AsNoTracking()
            .Where(x => x.StatDate >= startDate && x.StatDate <= endDate);
        if (!string.IsNullOrWhiteSpace(input.AppCode))
        {
            query = query.Where(x => x.AppCode == input.AppCode);
        }

        return await query.OrderBy(x => x.StatDate)
            .Select(x => new TrendPointOutput
            {
                StatDate = x.StatDate,
                AppCode = x.AppCode,
                Value = x.NewUserCount
            }).ToListAsync();
    }

    public async Task<List<TrendPointOutput>> GetActivityTrendAsync(GetTrendInput input)
    {
        var (startDate, endDate) = NormalizeDateRange(input);
        var query = _db.DailyUserStat.AsNoTracking()
            .Where(x => x.StatDate >= startDate && x.StatDate <= endDate);
        if (!string.IsNullOrWhiteSpace(input.AppCode))
        {
            query = query.Where(x => x.AppCode == input.AppCode);
        }

        return await query.OrderBy(x => x.StatDate)
            .Select(x => new TrendPointOutput
            {
                StatDate = x.StatDate,
                AppCode = x.AppCode,
                Value = x.ActiveUserCount
            }).ToListAsync();
    }

    public async Task<List<TrendPointOutput>> GetBusinessTrendAsync(GetTrendInput input)
    {
        var (startDate, endDate) = NormalizeDateRange(input);
        var query = _db.DailyBusinessStat.AsNoTracking()
            .Where(x => x.StatDate >= startDate && x.StatDate <= endDate);
        if (!string.IsNullOrWhiteSpace(input.AppCode))
        {
            query = query.Where(x => x.AppCode == input.AppCode);
        }

        return await query.OrderBy(x => x.StatDate)
            .Select(x => new TrendPointOutput
            {
                StatDate = x.StatDate,
                AppCode = x.AppCode,
                MetricCode = x.MetricCode,
                Value = x.MetricValue
            }).ToListAsync();
    }

    private static (DateTime startDate, DateTime endDate) NormalizeDateRange(GetTrendInput input)
    {
        var endDate = (input.EndDate ?? DateTime.UtcNow.Date).Date;
        var startDate = (input.StartDate ?? endDate.AddDays(-29)).Date;
        return (startDate, endDate);
    }
}
