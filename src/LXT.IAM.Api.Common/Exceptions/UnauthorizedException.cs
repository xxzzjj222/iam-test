using LXT.IAM.Api.Common.Exceptions.Base;
using Microsoft.AspNetCore.Http;

namespace LXT.IAM.Api.Common.Exceptions;

public class UnauthorizedException : ApiBaseException
{
    public UnauthorizedException(string message, params object[] arguments)
        : base(InternalErrorCode.UnauthorizedException, StatusCodes.Status401Unauthorized, arguments)
    {
        Message = message;
        Status = "UNAUTHORIZED";
    }
}
