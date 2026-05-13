using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Bll.Services.Dashboard.Dtos;

namespace LXT.IAM.Api.Bll.Services.Dashboard;

public interface IDashboardService : IScopedDependency
{
    Task<DashboardOverviewOutput> GetOverviewAsync();
    Task<List<TrendPointOutput>> GetUserTrendAsync(GetTrendInput input);
    Task<List<TrendPointOutput>> GetActivityTrendAsync(GetTrendInput input);
    Task<List<TrendPointOutput>> GetBusinessTrendAsync(GetTrendInput input);
}
