using LXT.IAM.Api.Common.Intefaces.Base;

namespace LXT.IAM.Api.Bll.Services.Sms;

/// <summary>
/// 短信发送接口
/// </summary>
public interface ISmsSender : IScopedDependency
{
    /// <summary>
    /// 发送验证码短信
    /// </summary>
    Task SendVerifyCodeAsync(string phone, string code);
}
