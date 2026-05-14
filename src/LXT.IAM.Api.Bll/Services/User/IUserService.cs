using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Bll.Services.User.Dtos;

namespace LXT.IAM.Api.Bll.Services.User;

public interface IUserService : IScopedDependency
{
    Task<PagedList<UserOutput>> GetPagedListAsync(GetUserPagedListInput input);
    Task<UserOutput> GetAsync(Guid UserId);
    Task FreezeAsync(Guid UserId);
    Task UnfreezeAsync(Guid UserId);
    Task AssignAppsAsync(Guid UserId, AssignUserAppsInput input);
    Task ResetPasswordAsync(Guid UserId, ResetPasswordInput input);
}

