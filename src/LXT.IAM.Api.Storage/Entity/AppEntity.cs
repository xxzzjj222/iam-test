using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("app")]
public class AppEntity : AuditEntity
{
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("code")]
    public string Code { get; set; } = string.Empty;

    [Column("category")]
    public string Category { get; set; } = string.Empty;

    [Column("client_type")]
    public string ClientType { get; set; } = string.Empty;

    [Column("auto_grant_for_normal_user")]
    public bool AutoGrantForNormalUser { get; set; }

    [Column("sort")]
    public int Sort { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("description")]
    public string? Description { get; set; }
}
