using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Bll.Services.PlatformFunction.Dtos;

namespace LXT.IAM.Api.Bll.Services.PlatformFunction;

public interface IPlatformFunctionService : IScopedDependency
{
    Task<List<PlatformFunctionOutput>> GetTreeAsync();
    Task<List<PlatformFunctionOutput>> GetCurrentUserFunctionsAsync();
    Task<PlatformFunctionOutput> GetAsync(long id);
    Task<long> AddAsync(AddPlatformFunctionInput input);
    Task PutAsync(long id, PutPlatformFunctionInput input);
    Task DeleteAsync(long id);
    Task AssignRoleFunctionsAsync(AssignPlatformRoleFunctionsInput input);
}
