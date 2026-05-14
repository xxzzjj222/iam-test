namespace LXT.IAM.Api.Bll.Services.OpenStat.Dtos;

public class ReportRoleSnapshotInput
{
    public Guid UserId { get; set; }

    public string AppCode { get; set; } = string.Empty;

    public string RoleCode { get; set; } = string.Empty;

    public string RoleName { get; set; } = string.Empty;

    public string? SourceRefId { get; set; }
}

