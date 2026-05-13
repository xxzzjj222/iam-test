using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("daily_business_stat")]
public class DailyBusinessStatEntity : AuditEntity
{
    [Column("stat_date")]
    public DateTime StatDate { get; set; }

    [Column("app_code")]
    public string AppCode { get; set; } = string.Empty;

    [Column("metric_code")]
    public string MetricCode { get; set; } = string.Empty;

    [Column("metric_name")]
    public string MetricName { get; set; } = string.Empty;

    [Column("metric_value")]
    public long MetricValue { get; set; }
}
