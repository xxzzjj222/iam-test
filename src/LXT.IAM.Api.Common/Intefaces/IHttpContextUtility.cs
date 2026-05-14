using LXT.IAM.Api.Common.Models;

namespace LXT.IAM.Api.Common.Intefaces;

/// <summary>
/// HttpContext 工具接口
/// </summary>
public interface IHttpContextUtility
{
    /// <summary>
    /// 获取当前认证用户信息
    /// </summary>
    AuthUserInfoBusiness GetUserInfo();

    /// <summary>
    /// 获取当前用户编号
    /// </summary>
    Guid GetUserId();
}
