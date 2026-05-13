namespace LXT.IAM.Api.Bll.Services.PlatformFunction.Dtos;

public class AssignPlatformRoleFunctionsInput
{
    public long RoleId { get; set; }
    public List<long> FunctionIds { get; set; } = new();
}
