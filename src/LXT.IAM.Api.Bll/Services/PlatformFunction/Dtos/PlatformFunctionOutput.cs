namespace LXT.IAM.Api.Bll.Services.PlatformFunction.Dtos;

public class PlatformFunctionOutput
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public long? ParentId { get; set; }
    public string? Icon { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? RouteUrl { get; set; }
    public string? ComponentUrl { get; set; }
    public bool IsHidden { get; set; }
    public int Sort { get; set; }
    public List<PlatformFunctionOutput> Children { get; set; } = new();
}
