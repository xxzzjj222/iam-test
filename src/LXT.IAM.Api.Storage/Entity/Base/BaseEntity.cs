using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity.Base;

public class BaseEntity
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("remark")]
    public string? Remark { get; set; }
}
