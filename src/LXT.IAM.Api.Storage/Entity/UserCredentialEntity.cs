using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("user_credential")]
public class UserCredentialEntity : AuditEntity
{
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("credential_type")]
    public string CredentialType { get; set; } = string.Empty;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("password_version")]
    public string? PasswordVersion { get; set; }

    [Column("need_reset_password")]
    public bool NeedResetPassword { get; set; }

    [Column("last_password_change_time")]
    public DateTime? LastPasswordChangeTime { get; set; }
}

