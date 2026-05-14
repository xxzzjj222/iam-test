namespace LXT.IAM.Api.Bll.Services.SocialAuth.Dtos;

/// <summary>
/// 第三方手机号信息
/// </summary>
public class SocialPhoneInfoOutput
{
    /// <summary>
    /// 手机号
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 区号
    /// </summary>
    public string? CountryCode { get; set; }

    /// <summary>
    /// 纯手机号
    /// </summary>
    public string? PurePhoneNumber { get; set; }
}
