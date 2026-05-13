using LXT.IAM.Api.Bll.Services.OpenAuth;
using LXT.IAM.Api.Bll.Services.OpenAuth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers;

[ApiController]
[Route("api/open-auth")]
public class OpenAuthController : ControllerBase
{
    private readonly IOpenAuthService _openAuthService;

    public OpenAuthController(IOpenAuthService openAuthService)
    {
        _openAuthService = openAuthService;
    }

    [AllowAnonymous]
    [HttpPost("token")]
    public async Task<ClientCredentialTokenOutput> GetClientTokenAsync([FromBody] ClientCredentialTokenInput input)
    {
        return await _openAuthService.GetClientTokenAsync(input);
    }

    [AllowAnonymous]
    [HttpPost("introspect")]
    public async Task<IntrospectTokenOutput> IntrospectAsync([FromBody] IntrospectTokenInput input)
    {
        return await _openAuthService.IntrospectAsync(input);
    }
}
