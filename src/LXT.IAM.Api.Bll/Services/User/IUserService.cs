using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Bll.Services.User.Dtos;

namespace LXT.IAM.Api.Bll.Services.User;

public interface IUserService : IScopedDependency
{
    Task<PagedList<UserOutput>> GetPagedListAsync(GetUserPagedListInput input);
    Task<UserOutput> GetAsync(Guid commonUserId);
    Task FreezeAsync(Guid commonUserId);
    Task UnfreezeAsync(Guid commonUserId);
    Task AssignAppsAsync(Guid commonUserId, AssignUserAppsInput input);
    Task ResetPasswordAsync(Guid commonUserId, ResetPasswordInput input);
}
