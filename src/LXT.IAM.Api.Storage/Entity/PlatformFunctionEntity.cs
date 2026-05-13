using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("platform_function")]
public class PlatformFunctionEntity : AuditEntity
{
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("code")]
    public string Code { get; set; } = string.Empty;

    [Column("parent_id")]
    public long? ParentId { get; set; }

    [Column("icon")]
    public string? Icon { get; set; }

    [Column("type")]
    public string Type { get; set; } = string.Empty;

    [Column("route_url")]
    public string? RouteUrl { get; set; }

    [Column("component_url")]
    public string? ComponentUrl { get; set; }

    [Column("is_hidden")]
    public bool IsHidden { get; set; }

    [Column("sort")]
    public int Sort { get; set; }
}
