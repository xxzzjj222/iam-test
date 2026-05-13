namespace LXT.IAM.Api.Bll.Services.User.Dtos;

public class AssignUserAppsInput
{
    public List<long> AppIds { get; set; } = new();
}
