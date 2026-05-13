namespace LXT.IAM.Api.Bll.Services.Auth.Dtos;

public class LoginByPasswordInput
{
    public string Account { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public string? CountryCode { get; set; }
    public string Password { get; set; } = string.Empty;
    public string AppCode { get; set; } = string.Empty;
    public string ClientType { get; set; } = string.Empty;
}
