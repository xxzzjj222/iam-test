using LXT.IAM.Api.Common.Intefaces.Base;

namespace LXT.IAM.Api.Bll.Services.DailyStat;

public interface IDailyStatService : IScopedDependency
{
    Task RefreshDailyUserStatAsync(DateTime? statDate = null);
}
