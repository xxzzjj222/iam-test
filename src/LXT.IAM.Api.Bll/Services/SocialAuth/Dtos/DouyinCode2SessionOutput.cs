namespace LXT.IAM.Api.Bll.Services.SocialAuth.Dtos;

/// <summary>
/// 抖音 code2session 输出
/// </summary>
public class DouyinCode2SessionOutput
{
    /// <summary>
    /// OpenId
    /// </summary>
    public string OpenId { get; set; } = string.Empty;

    /// <summary>
    /// SessionKey
    /// </summary>
    public string SessionKey { get; set; } = string.Empty;
}
