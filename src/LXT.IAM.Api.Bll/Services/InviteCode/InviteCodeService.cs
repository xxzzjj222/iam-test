using LXT.IAM.Api.Bll.Services.InviteCode.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Common.Exceptions;
using LXT.IAM.Api.Common.Helper;
using LXT.IAM.Api.Common.Intefaces;
using LXT.IAM.Api.Storage.Context;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Bll.Services.InviteCode;

public class InviteCodeService : IInviteCodeService
{
    private readonly IAMDbContext _db;
    private readonly IHttpContextUtility _httpContextUtility;

    public InviteCodeService(IAMDbContext db, IHttpContextUtility httpContextUtility)
    {
        _db = db;
        _httpContextUtility = httpContextUtility;
    }

    public async Task<List<string>> GetMyInviteCodesAsync()
    {
        var commonUserId = _httpContextUtility.GetUserId();
        return await _db.InviteCode.Where(x => x.CommonUserId == commonUserId && x.Status == CommonStatusConst.Enabled)
            .OrderByDescending(x => x.IsDefault)
            .ThenBy(x => x.Code)
            .Select(x => x.Code)
            .ToListAsync();
    }

    public async Task<ResolveInviteCodeOutput> ResolveAsync(string code)
    {
        var existCode = await _db.InviteCode.FirstOrDefaultAsync(x => x.Code == code && x.Status == CommonStatusConst.Enabled);
        if (existCode != null)
        {
            var inviter = await _db.CommonUser.FirstAsync(x => x.CommonUserId == existCode.CommonUserId);
            return new ResolveInviteCodeOutput
            {
                InviterUserId = existCode.CommonUserId,
                InviterName = inviter.Name,
                CodeType = existCode.CodeType,
                AppCode = existCode.AppCode,
                BizRoleCode = existCode.BizRoleCode
            };
        }

        var (success, _, prefix) = InviteCodeHelper.TryParse(code);
        if (!success)
        {
            throw new InvalidParameterException("邀请码无效");
        }

        return new ResolveInviteCodeOutput
        {
            InviterUserId = Guid.Empty,
            InviterName = string.Empty,
            CodeType = prefix == "U" ? AuthConst.CodeTypeUniversal : AuthConst.CodeTypeLegacyAlias
        };
    }
}
