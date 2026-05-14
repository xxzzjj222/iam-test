using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("user_app_role_snapshot")]
public class UserAppRoleSnapshotEntity : AuditEntity
{
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("app_code")]
    public string AppCode { get; set; } = string.Empty;

    [Column("role_code")]
    public string RoleCode { get; set; } = string.Empty;

    [Column("role_name")]
    public string RoleName { get; set; } = string.Empty;

    [Column("source_ref_id")]
    public string? SourceRefId { get; set; }

    [Column("sync_time")]
    public DateTime SyncTime { get; set; }
}

