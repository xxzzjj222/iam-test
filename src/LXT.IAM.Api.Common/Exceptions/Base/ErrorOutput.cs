namespace LXT.IAM.Api.Common.Exceptions.Base;

public class ErrorOutput
{
    public InternalErrorCode ErrorCode { get; set; }

    public string Message { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Detail { get; set; } = string.Empty;
}
