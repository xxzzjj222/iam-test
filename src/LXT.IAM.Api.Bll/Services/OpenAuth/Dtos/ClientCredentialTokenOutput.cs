namespace LXT.IAM.Api.Bll.Services.OpenAuth.Dtos;

public class ClientCredentialTokenOutput
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTime ExpireTime { get; set; }

    public string ClientCode { get; set; } = string.Empty;

    public string? Scopes { get; set; }
}
