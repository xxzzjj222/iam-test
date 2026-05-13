using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("oauth_client")]
public class OauthClientEntity : AuditEntity
{
    [Column("client_code")]
    public string ClientCode { get; set; } = string.Empty;

    [Column("client_secret_hash")]
    public string ClientSecretHash { get; set; } = string.Empty;

    [Column("client_name")]
    public string ClientName { get; set; } = string.Empty;

    [Column("grant_type")]
    public string GrantType { get; set; } = string.Empty;

    [Column("scopes")]
    public string? Scopes { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("access_token_expire_seconds")]
    public int AccessTokenExpireSeconds { get; set; }
}
