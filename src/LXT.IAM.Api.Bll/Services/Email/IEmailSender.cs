using LXT.IAM.Api.Common.Intefaces.Base;

namespace LXT.IAM.Api.Bll.Services.Email;

/// <summary>
/// 邮件发送接口
/// </summary>
public interface IEmailSender : IScopedDependency
{
    /// <summary>
    /// 发送验证码邮件
    /// </summary>
    Task SendVerifyCodeAsync(string email, string code);
}
