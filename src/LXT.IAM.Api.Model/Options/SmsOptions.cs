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
}
