using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Bll.Services.OpenUser.Dtos;

namespace LXT.IAM.Api.Bll.Services.OpenUser;

public interface IOpenUserService : IScopedDependency
{
    Task<List<OpenUserOutput>> GetUsersByAppAsync(string appCode);
    Task<OpenUserOutput> GetByCommonUserIdAsync(Guid commonUserId);
    Task<List<OpenUserOutput>> BatchGetAsync(BatchOpenUserInput input);
}
