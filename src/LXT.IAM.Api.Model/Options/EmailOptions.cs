using Microsoft.Extensions.Options;

namespace LXT.IAM.Api.Model.Options;

/// <summary>
/// 邮箱配置模型
/// </summary>
public class EmailOptions : IOptions<EmailOptions>
{
    EmailOptions IOptions<EmailOptions>.Value => this;

    /// <summary>
    /// 是否启用邮箱发送
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// SMTP 主机
    /// </summary>
    public string PostfixServerIp { get; set; } = string.Empty;

    /// <summary>
    /// SMTP 端口
    /// </summary>
    public int PostfixServerPort { get; set; } = 465;

    /// <summary>
    /// 发件人名称
    /// </summary>
    public string CurrName { get; set; } = string.Empty;

    /// <summary>
    /// 发件邮箱
    /// </summary>
    public string CurrEmailAddr { get; set; } = string.Empty;

    /// <summary>
    /// 发件邮箱密码
    /// </summary>
    public string MailPwd { get; set; } = string.Empty;

    /// <summary>
    /// 发送频率时间窗口（分钟）
    /// </summary>
    public int ELimitTime { get; set; } = 10;

    /// <summary>
    /// 时间窗口内最大次数
    /// </summary>
    public int ElimitCount { get; set; } = 3;
}
