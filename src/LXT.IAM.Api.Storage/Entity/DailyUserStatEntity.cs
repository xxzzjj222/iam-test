using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("daily_user_stat")]
public class DailyUserStatEntity : AuditEntity
{
    [Column("stat_date")]
    public DateTime StatDate { get; set; }

    [Column("app_code")]
    public string AppCode { get; set; } = string.Empty;

    [Column("total_user_count")]
    public int TotalUserCount { get; set; }

    [Column("new_user_count")]
    public int NewUserCount { get; set; }

    [Column("active_user_count")]
    public int ActiveUserCount { get; set; }
}
