namespace LXT.IAM.Api.Common.Exceptions.Base;

public class ApiBaseException : Exception
{
    public ApiBaseException(InternalErrorCode errorCode, int httpCode, params object[] arguments)
    {
        ErrorCode = errorCode;
        HttpCode = httpCode;
        Arguments = arguments;
    }

    public InternalErrorCode ErrorCode { get; set; }

    public new string Message { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Detail { get; set; } = string.Empty;

    public int HttpCode { get; set; }

    public object[] Arguments { get; set; }
}
