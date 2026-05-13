using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Bll.Services.OauthClient.Dtos;

namespace LXT.IAM.Api.Bll.Services.OauthClient;

public interface IOauthClientService : IScopedDependency
{
    Task<PagedList<OauthClientOutput>> GetPagedListAsync(GetOauthClientPagedListInput input);
    Task<OauthClientOutput> GetAsync(long id);
    Task<long> AddAsync(AddOauthClientInput input);
    Task PutAsync(long id, PutOauthClientInput input);
    Task DeleteAsync(long id);
}
