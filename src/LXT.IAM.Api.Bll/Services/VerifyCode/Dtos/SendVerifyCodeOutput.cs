namespace LXT.IAM.Api.Bll.Services.VerifyCode.Dtos;

public class SendVerifyCodeOutput
{
    public string Receiver { get; set; } = string.Empty;
    public string SceneCode { get; set; } = string.Empty;
    public DateTime ExpireTime { get; set; }
    public string? DebugCode { get; set; }
}
