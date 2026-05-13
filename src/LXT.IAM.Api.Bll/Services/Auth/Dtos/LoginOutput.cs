namespace LXT.IAM.Api.Bll.Services.Auth.Dtos;

public class LoginOutput
{
    public Guid CommonUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpireTime { get; set; }
    public DateTime RefreshTokenExpireTime { get; set; }
}
