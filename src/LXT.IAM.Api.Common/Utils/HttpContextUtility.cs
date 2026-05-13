using LXT.IAM.Api.Common.Intefaces;
using LXT.IAM.Api.Common.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LXT.IAM.Api.Common.Utils;

public class HttpContextUtility : IHttpContextUtility
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextUtility(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public AuthUserInfoBusiness GetUserInfo()
    {
        var claims = _httpContextAccessor.HttpContext?.User?.Claims;
        var userId = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value ?? Guid.Empty.ToString();
        var userName = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
        var mobilePhone = claims?.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone)?.Value ?? string.Empty;
        var role = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty;

        return new AuthUserInfoBusiness
        {
            UserId = Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : Guid.Empty,
            UserName = userName,
            MobilePhone = mobilePhone,
            Role = role,
            AppKey = _httpContextAccessor.HttpContext?.Request.Headers["X-App-Key"].ToString() ?? string.Empty
        };
    }

    public Guid GetUserId()
    {
        return GetUserInfo().UserId;
    }
}
