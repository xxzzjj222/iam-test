using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LXT.IAM.Api.Service.Filters;

/// <summary>
/// Swagger 多语言请求头过滤器
/// </summary>
public class AcceptLanguageHeaderFilter : IOperationFilter
{
    /// <summary>
    /// 应用请求头定义
    /// </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
        {
            operation.Parameters = new List<OpenApiParameter>();
        }

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Accept-Language",
            In = ParameterLocation.Header,
            Description = "Localization language (e.g. en-US, zh-CN)",
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Default = new OpenApiString("zh-CN")
            }
        });
    }
}
