using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Bll.Services.App.Dtos;

namespace LXT.IAM.Api.Bll.Services.App;

public interface IAppService : IScopedDependency
{
    Task<PagedList<AppOutput>> GetPagedListAsync(GetAppPagedListInput input);
    Task<List<LongStringKV>> GetOptionsAsync();
}
