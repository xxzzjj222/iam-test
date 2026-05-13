using System.ComponentModel.DataAnnotations;

namespace LXT.IAM.Api.Bll.Services.PlatformRole.Dtos;

public class AddPlatformRoleInput
{
    public string? Remark { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }
}
