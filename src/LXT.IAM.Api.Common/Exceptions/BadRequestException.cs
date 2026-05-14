using LXT.IAM.Api.Common.Exceptions.Base;
using Microsoft.AspNetCore.Http;

namespace LXT.IAM.Api.Common.Exceptions;

public class BadRequestException : ApiBaseException
{
    public BadRequestException(string message, params object[] arguments)
        : base(InternalErrorCode.BadRequestException, StatusCodes.Status400BadRequest, arguments)
    {
        Message = message;
        Status = "BAD_REQUEST";
    }
}
