using LXT.IAM.Api.Bll.Services.OpenAuth.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Common.Helper;
using LXT.IAM.Api.Storage.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LXT.IAM.Api.Bll.Services.OpenAuth;

public class OpenAuthService : IOpenAuthService
{
    private readonly IAMDbContext _db;
    private readonly string _securityKey;

    public OpenAuthService(IAMDbContext db, IConfiguration configuration)
    {
        _db = db;
        _securityKey = configuration["Jwt:SecurityKey"] ?? throw new InvalidOperationException("Jwt:SecurityKey is missing");
    }

    public async Task<ClientCredentialTokenOutput> GetClientTokenAsync(ClientCredentialTokenInput input)
    {
        var client = await _db.OauthClient.FirstOrDefaultAsync(x =>
            x.ClientCode == input.ClientCode &&
            x.Status == CommonStatusConst.Enabled &&
            x.GrantType == "client_credentials");

        if (client == null || client.ClientSecretHash != SecurityHelper.Sha256(input.ClientSecret))
        {
            throw new InvalidOperationException("客户端认证失败");
        }

        var expireTime = DateTime.UtcNow.AddSeconds(client.AccessTokenExpireSeconds);
        var claims = new List<Claim>
        {
            new(ClaimConst.ClientCode, client.ClientCode),
            new(ClaimConst.TokenType, "client_credentials"),
            new(ClaimConst.Scope, client.Scopes ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: expireTime,
            signingCredentials: creds);

        return new ClientCredentialTokenOutput
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpireTime = expireTime,
            ClientCode = client.ClientCode,
            Scopes = client.Scopes
        };
    }

    public Task<IntrospectTokenOutput> IntrospectAsync(IntrospectTokenInput input)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(input.AccessToken))
        {
            return Task.FromResult(new IntrospectTokenOutput { Active = false });
        }

        var token = handler.ReadJwtToken(input.AccessToken);
        if (token.ValidTo < DateTime.UtcNow)
        {
            return Task.FromResult(new IntrospectTokenOutput { Active = false });
        }

        return Task.FromResult(new IntrospectTokenOutput
        {
            Active = true,
            ClientCode = token.Claims.FirstOrDefault(x => x.Type == ClaimConst.ClientCode)?.Value ?? string.Empty,
            Scope = token.Claims.FirstOrDefault(x => x.Type == ClaimConst.Scope)?.Value
        });
    }
}
