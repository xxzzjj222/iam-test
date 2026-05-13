using LXT.IAM.Api.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace LXT.IAM.Api.Bll.Services.OauthClient.Dtos;

public class AddOauthClientInput
{
    public string? Remark { get; set; }

    [Required, MaxLength(100)]
    public string ClientCode { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string ClientName { get; set; } = string.Empty;

    [Required]
    public string ClientSecret { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string GrantType { get; set; } = "client_credentials";

    [MaxLength(1000)]
    public string? Scopes { get; set; }

    public int Status { get; set; } = 1;

    public int AccessTokenExpireSeconds { get; set; } = 7200;
}
