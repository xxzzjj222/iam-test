using LXT.IAM.Api.Common.Intefaces;
using LXT.IAM.Api.Common.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LXT.IAM.Api.Common.Utils;

/// <summary>
/// HttpContext 工具类
/// </summary>
public class HttpContextUtility : IHttpContextUtility
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 构造
    /// </summary>
    public HttpContextUtility(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 获取当前认证用户信息
    /// </summary>
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

    /// <summary>
    /// 获取当前用户编号
    /// </summary>
    public Guid GetUserId()
    {
        return GetUserInfo().UserId;
    }
}
