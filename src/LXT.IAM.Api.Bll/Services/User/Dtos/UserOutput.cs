namespace LXT.IAM.Api.Bll.Services.User.Dtos;

public class UserOutput
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int Status { get; set; }
    public bool IsFrozen { get; set; }
    public string RegisterAppCode { get; set; } = string.Empty;
    public DateTime CreateTime { get; set; }
}

