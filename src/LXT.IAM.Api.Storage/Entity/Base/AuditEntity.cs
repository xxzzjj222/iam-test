using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity.Base;

public class AuditEntity : BaseEntity
{
    [Column("create_by")]
    public Guid CreateBy { get; set; }

    [Column("create_time")]
    public DateTime CreateTime { get; set; }

    [Column("update_by")]
    public Guid? UpdateBy { get; set; }

    [Column("update_time")]
    public DateTime? UpdateTime { get; set; }
}
