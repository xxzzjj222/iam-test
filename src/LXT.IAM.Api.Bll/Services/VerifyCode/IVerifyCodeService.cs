using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Bll.Services.VerifyCode.Dtos;

namespace LXT.IAM.Api.Bll.Services.VerifyCode;

public interface IVerifyCodeService : IScopedDependency
{
    Task<SendVerifyCodeOutput> SendAsync(SendVerifyCodeInput input);
}
