using LXT.IAM.Api.Bll.Services.VerifyCode;
using LXT.IAM.Api.Bll.Services.VerifyCode.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers;

[ApiController]
[Route("api/verify-code")]
public class VerifyCodeController : ControllerBase
{
    private readonly IVerifyCodeService _verifyCodeService;

    public VerifyCodeController(IVerifyCodeService verifyCodeService)
    {
        _verifyCodeService = verifyCodeService;
    }

    [AllowAnonymous]
    [HttpPost("send")]
    public async Task<SendVerifyCodeOutput> SendAsync([FromBody] SendVerifyCodeInput input)
    {
        return await _verifyCodeService.SendAsync(input);
    }
}
