using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Bll.Services.OpenStat.Dtos;

namespace LXT.IAM.Api.Bll.Services.OpenStat;

public interface IOpenStatService : IScopedDependency
{
    Task ReportUserActivityAsync(ReportUserActivityInput input);
    Task ReportBusinessMetricAsync(ReportBusinessMetricInput input);
    Task ReportRoleSnapshotAsync(ReportRoleSnapshotInput input);
}
