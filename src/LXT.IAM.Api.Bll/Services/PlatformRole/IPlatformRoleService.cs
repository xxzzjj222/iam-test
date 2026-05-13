using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Bll.Services.PlatformRole.Dtos;

namespace LXT.IAM.Api.Bll.Services.PlatformRole;

public interface IPlatformRoleService : IScopedDependency
{
    Task<PagedList<PlatformRoleOutput>> GetPagedListAsync(GetPlatformRolePagedListInput input);
    Task<PlatformRoleOutput> GetAsync(long id);
    Task<List<LongStringKV>> GetOptionsAsync();
    Task<long> AddAsync(AddPlatformRoleInput input);
    Task PutAsync(long id, PutPlatformRoleInput input);
    Task DeleteAsync(long id);
    Task AssignUserRolesAsync(AssignUserPlatformRolesInput input);
}
