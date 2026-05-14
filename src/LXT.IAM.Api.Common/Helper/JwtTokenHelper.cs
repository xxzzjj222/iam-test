using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Common.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LXT.IAM.Api.Common.Helper;

/// <summary>
/// JWT 令牌工具�?/// </summary>
public class JwtTokenHelper
{
    private readonly string _securityKey;

    /// <summary>
    /// 构�?    /// </summary>
    public JwtTokenHelper(IConfiguration configuration)
    {
        _securityKey = configuration["Jwt:SecurityKey"] ?? throw new InternalServerException("Jwt配置缺失");
    }

    /// <summary>
    /// 生成访问令牌
    /// </summary>
    public string GenerateAccessToken(Guid UserId, string userName, string? phone, string? email, Guid sessionId, string appCode, IEnumerable<string>? platformRoles = null, int expireHours = 2)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, UserId.ToString()),
            new(ClaimTypes.Name, userName),
            new(ClaimTypes.MobilePhone, phone ?? string.Empty),
            new(ClaimTypes.Email, email ?? string.Empty),
            new(ClaimConst.SessionId, sessionId.ToString()),
            new(ClaimConst.AppCode, appCode)
        };

        if (platformRoles != null)
        {
            claims.AddRange(platformRoles.Where(x => !string.IsNullOrWhiteSpace(x)).Select(role => new Claim(ClaimTypes.Role, role)));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expireHours),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

