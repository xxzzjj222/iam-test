namespace LXT.IAM.Api.Bll.Services.App.Dtos;

public class AppOutput
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ClientType { get; set; } = string.Empty;
    public bool AutoGrantForNormalUser { get; set; }
    public int Status { get; set; }
}
