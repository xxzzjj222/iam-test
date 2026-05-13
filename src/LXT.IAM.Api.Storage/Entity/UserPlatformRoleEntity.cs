using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("user_platform_role")]
public class UserPlatformRoleEntity : AuditEntity
{
    [Column("common_user_id")]
    public Guid CommonUserId { get; set; }

    [Column("role_id")]
    public long RoleId { get; set; }
}
