using LXT.IAM.Api.Bll.Services.VerifyCode;
using LXT.IAM.Api.Bll.Services.VerifyCode.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers;

/// <summary>
/// 验证码控制器
/// </summary>
[ApiController]
[Route("api/verify-code")]
public class VerifyCodeController : ControllerBase
{
    private readonly IVerifyCodeService _verifyCodeService;

    /// <summary>
    /// 构造
    /// </summary>
    public VerifyCodeController(IVerifyCodeService verifyCodeService)
    {
        _verifyCodeService = verifyCodeService;
    }

    /// <summary>
    /// 发送验证码
    /// </summary>
    [AllowAnonymous]
    [HttpPost("send")]
    public async Task<SendVerifyCodeOutput> SendAsync([FromBody] SendVerifyCodeInput input)
    {
        return await _verifyCodeService.SendAsync(input);
    }
}
