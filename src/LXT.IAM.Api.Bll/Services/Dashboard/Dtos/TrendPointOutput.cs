namespace LXT.IAM.Api.Bll.Services.Dashboard.Dtos;

public class TrendPointOutput
{
    public DateTime StatDate { get; set; }

    public long Value { get; set; }

    public string? AppCode { get; set; }

    public string? MetricCode { get; set; }
}
