using System.ComponentModel.DataAnnotations;

namespace LXT.IAM.Api.Bll.Services.Auth.Dtos;

public class ChangePasswordInput
{
    [Required]
    public string OldPassword { get; set; } = string.Empty;

    [Required]
    public string NewPassword { get; set; } = string.Empty;
}
