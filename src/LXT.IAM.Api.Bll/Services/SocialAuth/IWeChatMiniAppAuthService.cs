using LXT.IAM.Api.Bll.Services.SocialAuth.Dtos;
using LXT.IAM.Api.Common.Intefaces.Base;

namespace LXT.IAM.Api.Bll.Services.SocialAuth;

/// <summary>
/// 微信小程序认证服务
/// </summary>
public interface IWeChatMiniAppAuthService : IScopedDependency
{
    /// <summary>
    /// code 换 session
    /// </summary>
    Task<WeChatCode2SessionOutput> Code2SessionAsync(string code);

    /// <summary>
    /// 解密手机号
    /// </summary>
    Task<SocialPhoneInfoOutput> DecodePhoneAsync(DecodePhoneInput input);
}
