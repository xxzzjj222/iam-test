using LXT.IAM.Api.Common.Exceptions.Base;
using Microsoft.AspNetCore.Http;

namespace LXT.IAM.Api.Common.Exceptions;

public class InternalServerException : ApiBaseException
{
    public InternalServerException(string message, params object[] arguments)
        : base(InternalErrorCode.InternalServerError, StatusCodes.Status500InternalServerError, arguments)
    {
        Message = message;
        Status = "INTERNAL_SERVER_ERROR";
    }
}
