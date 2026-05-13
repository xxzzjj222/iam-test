using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("user_app")]
public class UserAppEntity : AuditEntity
{
    [Column("common_user_id")]
    public Guid CommonUserId { get; set; }

    [Column("app_id")]
    public long AppId { get; set; }

    [Column("grant_type")]
    public string GrantType { get; set; } = string.Empty;

    [Column("status")]
    public int Status { get; set; }
}
