namespace LXT.IAM.Api.Bll.Services.OpenStat.Dtos;

public class ReportBusinessMetricInput
{
    public DateTime? StatDate { get; set; }

    public string AppCode { get; set; } = string.Empty;

    public string MetricCode { get; set; } = string.Empty;

    public string MetricName { get; set; } = string.Empty;

    public long MetricValue { get; set; }
}
