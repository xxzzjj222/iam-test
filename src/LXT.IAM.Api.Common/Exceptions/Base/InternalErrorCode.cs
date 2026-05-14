using System.ComponentModel;

namespace LXT.IAM.Api.Common.Exceptions.Base;

public enum InternalErrorCode
{
    [Description("内部服务错误")]
    InternalServerError = 500500,

    [Description("未找到")]
    NotFoundException = 400404,

    [Description("请求错误")]
    BadRequestException = 400400,

    [Description("参数无效")]
    InvalidParameterException = 400403,

    [Description("未授权")]
    UnauthorizedException = 400401
}
