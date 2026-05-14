namespace LXT.IAM.Api.Bll.Services.Auth.Dtos;

public class CurrentUserOutput
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public List<string> InviteCodes { get; set; } = new();
}

