namespace LXT.IAM.Api.Bll.Services.OauthClient.Dtos;

public class OauthClientOutput
{
    public long Id { get; set; }
    public string ClientCode { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string GrantType { get; set; } = string.Empty;
    public string? Scopes { get; set; }
    public int Status { get; set; }
    public int AccessTokenExpireSeconds { get; set; }
}
