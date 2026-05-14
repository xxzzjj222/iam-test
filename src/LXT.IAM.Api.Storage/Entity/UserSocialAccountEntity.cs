using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("user_social_account")]
public class UserSocialAccountEntity : AuditEntity
{
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("platform_type")]
    public string PlatformType { get; set; } = string.Empty;

    [Column("app_id")]
    public string AppId { get; set; } = string.Empty;

    [Column("open_id")]
    public string OpenId { get; set; } = string.Empty;

    [Column("union_id")]
    public string? UnionId { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("bind_time")]
    public DateTime BindTime { get; set; }

    [Column("unbind_time")]
    public DateTime? UnbindTime { get; set; }
}

