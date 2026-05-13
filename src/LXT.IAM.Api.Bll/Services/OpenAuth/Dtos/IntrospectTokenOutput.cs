namespace LXT.IAM.Api.Bll.Services.OpenAuth.Dtos;

public class IntrospectTokenOutput
{
    public bool Active { get; set; }

    public string ClientCode { get; set; } = string.Empty;

    public string? Scope { get; set; }
}
