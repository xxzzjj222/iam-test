using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("user_app_activity_daily")]
public class UserAppActivityDailyEntity : AuditEntity
{
    [Column("stat_date")]
    public DateTime StatDate { get; set; }

    [Column("app_code")]
    public string AppCode { get; set; } = string.Empty;

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("active_times")]
    public int ActiveTimes { get; set; }

    [Column("last_active_time")]
    public DateTime LastActiveTime { get; set; }
}

