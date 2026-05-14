using LXT.IAM.Api.Bll.Services.InviteCode;
using LXT.IAM.Api.Bll.Services.InviteCode.Dtos;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

/// <summary>
/// 邀请码控制器
/// </summary>
[Route("api/invite-code")]
public class InviteCodeController : BusinessController
{
    private readonly IInviteCodeService _inviteCodeService;

    /// <summary>
    /// 构造
    /// </summary>
    public InviteCodeController(IInviteCodeService inviteCodeService)
    {
        _inviteCodeService = inviteCodeService;
    }

    /// <summary>
    /// 获取当前用户的邀请码列表
    /// </summary>
    [HttpGet("my")]
    public async Task<List<string>> GetMyInviteCodesAsync()
    {
        return await _inviteCodeService.GetMyInviteCodesAsync();
    }

    /// <summary>
    /// 解析邀请码
    /// </summary>
    [HttpGet("resolve/{code}")]
    public async Task<ResolveInviteCodeOutput> ResolveAsync(string code)
    {
        return await _inviteCodeService.ResolveAsync(code);
    }
}
