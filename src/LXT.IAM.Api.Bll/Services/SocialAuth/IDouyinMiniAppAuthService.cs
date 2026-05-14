using LXT.IAM.Api.Bll.Services.SocialAuth.Dtos;
using LXT.IAM.Api.Common.Intefaces.Base;

namespace LXT.IAM.Api.Bll.Services.SocialAuth;

/// <summary>
/// 抖音小程序认证服务
/// </summary>
public interface IDouyinMiniAppAuthService : IScopedDependency
{
    /// <summary>
    /// code 换 session
    /// </summary>
    Task<DouyinCode2SessionOutput> Code2SessionAsync(string code);
}
