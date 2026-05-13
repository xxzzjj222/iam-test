using LXT.IAM.Api.Common.Consts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LXT.IAM.Api.Common.Helper;

public class JwtTokenHelper
{
    private readonly string _securityKey;

    public JwtTokenHelper(IConfiguration configuration)
    {
        _securityKey = configuration["Jwt:SecurityKey"] ?? throw new InvalidOperationException("Jwt:SecurityKey is missing");
    }

    public string GenerateAccessToken(Guid commonUserId, string userName, string? phone, string? email, Guid sessionId, string appCode, IEnumerable<string>? platformRoles = null, int expireHours = 2)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, commonUserId.ToString()),
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
