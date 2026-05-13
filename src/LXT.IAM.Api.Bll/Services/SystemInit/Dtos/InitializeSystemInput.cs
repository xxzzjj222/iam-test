using System.ComponentModel.DataAnnotations;

namespace LXT.IAM.Api.Bll.Services.SystemInit.Dtos;

public class InitializeSystemInput
{
    [Required]
    public string AdminAccountType { get; set; } = string.Empty;

    [Required]
    public string AdminAccount { get; set; } = string.Empty;

    public string? CountryCode { get; set; }

    [Required]
    public string AdminPassword { get; set; } = string.Empty;

    public string AdminName { get; set; } = "super-admin";
}
