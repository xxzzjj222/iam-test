using LXT.IAM.Api.Common.Exceptions.Base;
using Microsoft.AspNetCore.Http;

namespace LXT.IAM.Api.Common.Exceptions;

public class NotFoundException : ApiBaseException
{
    public NotFoundException(string message, params object[] arguments)
        : base(InternalErrorCode.NotFoundException, StatusCodes.Status404NotFound, arguments)
    {
        Message = message;
        Status = "NOT_FOUND";
    }
}
