using Microsoft.Extensions.Options;

namespace LXT.IAM.Api.Model.Options;

/// <summary>
/// 短信配置模型
/// </summary>
public class SmsOptions : IOptions<SmsOptions>
{
    SmsOptions IOptions<SmsOptions>.Value => this;

    /// <summary>
    /// 是否启用短信发送
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 阿里云 AccessKeyId
    /// </summary>
    public string ALIBABA_CLOUD_ACCESS_KEY_ID { get; set; } = string.Empty;

    /// <summary>
    /// 阿里云 AccessKeySecret
    /// </summary>
    public string ALIBABA_CLOUD_ACCESS_KEY_SECRET { get; set; } = string.Empty;

    /// <summary>
    /// 短信签名
    /// </summary>
    public string ALIBABA_CLOUD_ACCESS_SIGNNAME { get; set; } = string.Empty;

    /// <summary>
    /// 模板编码
    /// </summary>
    public string ALIBABA_CLOUD_ACCESS_TEMPLATECODE { get; set; } = string.Empty;

    /// <summary>
    /// 发送间隔秒数
    /// </summary>
    public int SendIntervalSeconds { get; set; } = 59;

    /// <summary>
    /// 单位时间窗口（分钟）
    /// </summary>
    public int LimitTimeMinutes { get; set; } = 10;

    /// <summary>
    /// 单位时间窗口内最大次数
    /// </summary>
    public int LimitCount { get; set; } = 3;

    /// <summary>
    /// 短信模板文本
    /// </summary>
    public string TemplateText { get; set; } = "您的验证码是 {code}，10分钟内有效。";
}
