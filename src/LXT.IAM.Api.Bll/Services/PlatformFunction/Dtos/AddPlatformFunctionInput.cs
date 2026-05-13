using LXT.IAM.Api.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace LXT.IAM.Api.Bll.Services.PlatformFunction.Dtos;

public class AddPlatformFunctionInput
{
    public string? Remark { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required, MaxLength(100)]
    public string Code { get; set; } = string.Empty;
    public long? ParentId { get; set; }
    public string? Icon { get; set; }
    [Required, MaxLength(50)]
    public string Type { get; set; } = "MENU";
    public string? RouteUrl { get; set; }
    public string? ComponentUrl { get; set; }
    public bool IsHidden { get; set; }
    public int Sort { get; set; }
}
