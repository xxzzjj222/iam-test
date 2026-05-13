using LXT.IAM.Api.Common.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LXT.IAM.Api.Service.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequireScopeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _scope;

    public RequireScopeAttribute(string scope)
    {
        _scope = scope;
    }

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var scopes = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimConst.Scope)?.Value;
        var tokenType = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimConst.TokenType)?.Value;

        if (tokenType == AuthConst.TokenTypeClientCredentials)
        {
            var scopeList = (scopes ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (!scopeList.Contains(_scope, StringComparer.OrdinalIgnoreCase))
            {
                context.Result = new ForbidResult();
            }
        }

        return Task.CompletedTask;
    }
}
