using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("user_platform_role")]
public class UserPlatformRoleEntity : AuditEntity
{
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("role_id")]
    public long RoleId { get; set; }
}

