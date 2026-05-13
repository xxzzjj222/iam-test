using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("invite_relation")]
public class InviteRelationEntity : AuditEntity
{
    [Column("inviter_user_id")]
    public Guid InviterUserId { get; set; }

    [Column("invitee_user_id")]
    public Guid InviteeUserId { get; set; }

    [Column("invite_code_id")]
    public long InviteCodeId { get; set; }

    [Column("source_app_code")]
    public string SourceAppCode { get; set; } = string.Empty;

    [Column("register_app_code")]
    public string RegisterAppCode { get; set; } = string.Empty;

    [Column("resolved_biz_role_code")]
    public string? ResolvedBizRoleCode { get; set; }

    [Column("resolved_external_ref_id")]
    public string? ResolvedExternalRefId { get; set; }

    [Column("bind_time")]
    public DateTime BindTime { get; set; }
}
