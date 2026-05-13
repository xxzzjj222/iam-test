namespace LXT.IAM.Api.Bll.Services.VerifyCode.Dtos;

public class SendVerifyCodeInput
{
    public string Receiver { get; set; } = string.Empty;
    public string ReceiverType { get; set; } = string.Empty;
    public string SceneCode { get; set; } = string.Empty;
    public string? CountryCode { get; set; }
}
