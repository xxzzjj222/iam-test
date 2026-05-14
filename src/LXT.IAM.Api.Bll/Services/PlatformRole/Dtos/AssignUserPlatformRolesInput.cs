namespace LXT.IAM.Api.Bll.Services.PlatformRole.Dtos;

public class AssignUserPlatformRolesInput
{
    public Guid UserId { get; set; }
    public List<long> RoleIds { get; set; } = new();
}

