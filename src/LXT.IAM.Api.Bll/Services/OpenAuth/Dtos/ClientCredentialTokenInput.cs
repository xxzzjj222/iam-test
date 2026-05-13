namespace LXT.IAM.Api.Bll.Services.OpenAuth.Dtos;

public class ClientCredentialTokenInput
{
    public string ClientCode { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;
}
