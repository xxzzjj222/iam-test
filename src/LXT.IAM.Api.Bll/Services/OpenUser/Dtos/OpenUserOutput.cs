namespace LXT.IAM.Api.Bll.Services.OpenUser.Dtos;

public class OpenUserOutput
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Avatar { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool IsFrozen { get; set; }

    public string RegisterAppCode { get; set; } = string.Empty;
}

