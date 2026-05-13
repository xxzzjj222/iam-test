using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Bll.Services.OpenAuth.Dtos;

namespace LXT.IAM.Api.Bll.Services.OpenAuth;

public interface IOpenAuthService : IScopedDependency
{
    Task<ClientCredentialTokenOutput> GetClientTokenAsync(ClientCredentialTokenInput input);
    Task<IntrospectTokenOutput> IntrospectAsync(IntrospectTokenInput input);
}
