namespace LXT.IAM.Api.Bll.Services.InviteCode.Dtos;

public class ResolveInviteCodeOutput
{
    public Guid InviterUserId { get; set; }
    public string InviterName { get; set; } = string.Empty;
    public string CodeType { get; set; } = string.Empty;
    public string? AppCode { get; set; }
    public string? BizRoleCode { get; set; }
}
