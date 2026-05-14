namespace LXT.IAM.Api.Bll.Services.SocialAuth.Dtos;

/// <summary>
/// 抖音小程序登录请求
/// </summary>
public class DouyinMiniAppLoginInput
{
    /// <summary>
    /// 抖音 code
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// IV
    /// </summary>
    public string IV { get; set; } = string.Empty;

    /// <summary>
    /// 加密数据
    /// </summary>
    public string EncryptedData { get; set; } = string.Empty;

    /// <summary>
    /// 应用编码
    /// </summary>
    public string AppCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端类型
    /// </summary>
    public string ClientType { get; set; } = string.Empty;

    /// <summary>
    /// 邀请码
    /// </summary>
    public string? InviteCode { get; set; }
}
