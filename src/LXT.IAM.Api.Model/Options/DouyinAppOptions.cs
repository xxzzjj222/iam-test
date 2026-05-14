using Microsoft.Extensions.Options;

namespace LXT.IAM.Api.Model.Options;

/// <summary>
/// 抖音小程序配置模型
/// </summary>
public class DouyinAppOptions : IOptions<DouyinAppOptions>
{
    DouyinAppOptions IOptions<DouyinAppOptions>.Value => this;

    /// <summary>
    /// 抖音小程序 AppId
    /// </summary>
    public string AppId { get; set; } = string.Empty;

    /// <summary>
    /// 抖音小程序 AppSecret
    /// </summary>
    public string AppSecret { get; set; } = string.Empty;

    /// <summary>
    /// AccessToken 保留字段
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
}
