using System.ComponentModel.DataAnnotations;

namespace LXT.IAM.Api.Bll.Services.User.Dtos;

public class ResetPasswordInput
{
    [Required]
    public string NewPassword { get; set; } = string.Empty;

    public bool NeedResetPassword { get; set; }
}
