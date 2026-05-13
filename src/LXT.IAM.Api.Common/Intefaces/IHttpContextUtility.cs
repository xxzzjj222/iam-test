using LXT.IAM.Api.Common.Models;

namespace LXT.IAM.Api.Common.Intefaces;

public interface IHttpContextUtility
{
    AuthUserInfoBusiness GetUserInfo();

    Guid GetUserId();
}
