namespace LXT.IAM.Api.Common.Models;

/// <summary>
/// 当前认证用户业务模型
/// </summary>
public class AuthUserInfoBusiness
{
    public Guid UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string MobilePhone { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string AppKey { get; set; } = string.Empty;
}
