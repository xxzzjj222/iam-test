using LXT.IAM.Api.Bll.Services.OpenStat.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Storage.Context;
using LXT.IAM.Api.Storage.Entity;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Bll.Services.OpenStat;

public class OpenStatService : IOpenStatService
{
    private readonly IAMDbContext _db;

    public OpenStatService(IAMDbContext db)
    {
        _db = db;
    }

    public async Task ReportUserActivityAsync(ReportUserActivityInput input)
    {
        var activeTime = input.ActiveTime ?? DateTime.UtcNow;
        var statDate = activeTime.Date;
        var entity = await _db.UserAppActivityDaily.FirstOrDefaultAsync(x =>
            x.CommonUserId == input.CommonUserId &&
            x.AppCode == input.AppCode &&
            x.StatDate == statDate);

        if (entity == null)
        {
            entity = new UserAppActivityDailyEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                CommonUserId = input.CommonUserId,
                AppCode = input.AppCode,
                StatDate = statDate,
                ActiveTimes = 1,
                LastActiveTime = activeTime
            };
            _db.UserAppActivityDaily.Add(entity);
        }
        else
        {
            entity.ActiveTimes += 1;
            entity.LastActiveTime = activeTime;
        }

        var user = await _db.CommonUser.FirstOrDefaultAsync(x => x.CommonUserId == input.CommonUserId);
        if (user != null)
        {
            user.LastActiveTime = activeTime;
        }

        await _db.SaveChangesAsync();
    }

    public async Task ReportBusinessMetricAsync(ReportBusinessMetricInput input)
    {
        var statDate = (input.StatDate ?? DateTime.UtcNow).Date;
        var entity = await _db.DailyBusinessStat.FirstOrDefaultAsync(x =>
            x.StatDate == statDate &&
            x.AppCode == input.AppCode &&
            x.MetricCode == input.MetricCode);

        if (entity == null)
        {
            entity = new DailyBusinessStatEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                StatDate = statDate,
                AppCode = input.AppCode,
                MetricCode = input.MetricCode,
                MetricName = input.MetricName,
                MetricValue = input.MetricValue
            };
            _db.DailyBusinessStat.Add(entity);
        }
        else
        {
            entity.MetricName = input.MetricName;
            entity.MetricValue = input.MetricValue;
        }

        await _db.SaveChangesAsync();
    }

    public async Task ReportRoleSnapshotAsync(ReportRoleSnapshotInput input)
    {
        var entity = await _db.UserAppRoleSnapshot.FirstOrDefaultAsync(x =>
            x.CommonUserId == input.CommonUserId &&
            x.AppCode == input.AppCode &&
            x.RoleCode == input.RoleCode);

        if (entity == null)
        {
            entity = new UserAppRoleSnapshotEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                CommonUserId = input.CommonUserId,
                AppCode = input.AppCode,
                RoleCode = input.RoleCode,
                RoleName = input.RoleName,
                SourceRefId = input.SourceRefId,
                SyncTime = DateTime.UtcNow
            };
            _db.UserAppRoleSnapshot.Add(entity);
        }
        else
        {
            entity.RoleName = input.RoleName;
            entity.SourceRefId = input.SourceRefId;
            entity.SyncTime = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
    }
}
