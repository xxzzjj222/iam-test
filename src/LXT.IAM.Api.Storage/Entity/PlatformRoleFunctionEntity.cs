using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("platform_role_function")]
public class PlatformRoleFunctionEntity : AuditEntity
{
    [Column("role_id")]
    public long RoleId { get; set; }

    [Column("function_id")]
    public long FunctionId { get; set; }
}
