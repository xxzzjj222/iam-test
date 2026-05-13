using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("common_user")]
public class CommonUserEntity : AuditEntity
{
    [Column("common_user_id")]
    public Guid CommonUserId { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("avatar")]
    public string? Avatar { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("is_frozen")]
    public bool IsFrozen { get; set; }

    [Column("register_app_code")]
    public string RegisterAppCode { get; set; } = string.Empty;

    [Column("last_login_time")]
    public DateTime? LastLoginTime { get; set; }

    [Column("last_active_time")]
    public DateTime? LastActiveTime { get; set; }

    [Column("language_code")]
    public string? LanguageCode { get; set; }

    [Column("country_code")]
    public string? CountryCode { get; set; }
}
