using Microsoft.Extensions.Options;

namespace LXT.IAM.Api.Model.Options;

/// <summary>
/// 微信小程序配置模型
/// </summary>
public class AppOptions : IOptions<AppOptions>
{
    AppOptions IOptions<AppOptions>.Value => this;

    /// <summary>
    /// 微信小程序 AppId
    /// </summary>
    public string AppId { get; set; } = string.Empty;

    /// <summary>
    /// 微信小程序 AppSecret
    /// </summary>
    public string AppSecret { get; set; } = string.Empty;
}
