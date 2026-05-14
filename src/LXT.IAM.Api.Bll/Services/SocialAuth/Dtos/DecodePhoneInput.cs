namespace LXT.IAM.Api.Bll.Services.SocialAuth.Dtos;

/// <summary>
/// 解密手机号请求
/// </summary>
public class DecodePhoneInput
{
    /// <summary>
    /// SessionKey
    /// </summary>
    public string SessionKey { get; set; } = string.Empty;

    /// <summary>
    /// IV
    /// </summary>
    public string IV { get; set; } = string.Empty;

    /// <summary>
    /// 加密数据
    /// </summary>
    public string EncryptedData { get; set; } = string.Empty;
}
