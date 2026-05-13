namespace LXT.IAM.Api.Bll.Services.PlatformRole.Dtos;

public class PlatformRoleOutput
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
}
