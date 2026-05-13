using LXT.IAM.Api.Bll.Services.InviteCode;
using LXT.IAM.Api.Bll.Services.InviteCode.Dtos;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

[Route("api/invite-code")]
public class InviteCodeController : BusinessController
{
    private readonly IInviteCodeService _inviteCodeService;

    public InviteCodeController(IInviteCodeService inviteCodeService)
    {
        _inviteCodeService = inviteCodeService;
    }

    [HttpGet("my")]
    public async Task<List<string>> GetMyInviteCodesAsync()
    {
        return await _inviteCodeService.GetMyInviteCodesAsync();
    }

    [HttpGet("resolve/{code}")]
    public async Task<ResolveInviteCodeOutput> ResolveAsync(string code)
    {
        return await _inviteCodeService.ResolveAsync(code);
    }
}
