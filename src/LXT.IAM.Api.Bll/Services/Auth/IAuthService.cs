using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Bll.Services.Auth.Dtos;
using LXT.IAM.Api.Bll.Services.SocialAuth.Dtos;

namespace LXT.IAM.Api.Bll.Services.Auth;

public interface IAuthService : IScopedDependency
{
    Task<LoginOutput> LoginByPasswordAsync(LoginByPasswordInput input);
    Task<LoginOutput> LoginByCodeAsync(LoginByCodeInput input);
    Task<LoginOutput> RegisterAsync(RegisterInput input);
    Task<LoginOutput> RefreshTokenAsync(RefreshTokenInput input);
    Task<CurrentUserOutput> GetCurrentUserAsync();
    Task ChangePasswordAsync(ChangePasswordInput input);
    Task<LoginOutput> LoginByWeChatMiniAppAsync(WeChatMiniAppLoginInput input);
    Task<LoginOutput> LoginByDouyinMiniAppAsync(DouyinMiniAppLoginInput input);
}
