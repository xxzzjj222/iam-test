namespace LXT.IAM.Api.Bll.Services.Dashboard.Dtos;

public class DashboardOverviewOutput
{
    public int TotalUserCount { get; set; }
    public int NewUserCount { get; set; }
    public int ActiveUserCount { get; set; }
    public List<AppMetricOutput> Metrics { get; set; } = new();
}

public class AppMetricOutput
{
    public string AppCode { get; set; } = string.Empty;
    public string MetricCode { get; set; } = string.Empty;
    public string MetricName { get; set; } = string.Empty;
    public long MetricValue { get; set; }
}
