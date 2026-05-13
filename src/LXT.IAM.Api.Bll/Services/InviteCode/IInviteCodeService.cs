using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Bll.Services.InviteCode.Dtos;

namespace LXT.IAM.Api.Bll.Services.InviteCode;

public interface IInviteCodeService : IScopedDependency
{
    Task<List<string>> GetMyInviteCodesAsync();
    Task<ResolveInviteCodeOutput> ResolveAsync(string code);
}
