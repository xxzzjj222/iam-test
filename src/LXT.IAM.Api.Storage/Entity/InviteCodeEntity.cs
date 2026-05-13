using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("invite_code")]
public class InviteCodeEntity : AuditEntity
{
    [Column("common_user_id")]
    public Guid CommonUserId { get; set; }

    [Column("code")]
    public string Code { get; set; } = string.Empty;

    [Column("code_type")]
    public string CodeType { get; set; } = string.Empty;

    [Column("app_code")]
    public string? AppCode { get; set; }

    [Column("biz_role_code")]
    public string? BizRoleCode { get; set; }

    [Column("external_ref_id")]
    public string? ExternalRefId { get; set; }

    [Column("is_default")]
    public bool IsDefault { get; set; }

    [Column("status")]
    public int Status { get; set; }
}
