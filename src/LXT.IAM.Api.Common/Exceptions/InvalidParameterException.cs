using LXT.IAM.Api.Common.Exceptions.Base;
using Microsoft.AspNetCore.Http;

namespace LXT.IAM.Api.Common.Exceptions;

public class InvalidParameterException : ApiBaseException
{
    public InvalidParameterException(string message, params object[] arguments)
        : base(InternalErrorCode.InvalidParameterException, StatusCodes.Status403Forbidden, arguments)
    {
        Message = message;
        Status = "INVALID_PARAMETER";
    }
}
