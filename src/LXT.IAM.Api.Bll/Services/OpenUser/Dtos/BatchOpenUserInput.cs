namespace LXT.IAM.Api.Bll.Services.OpenUser.Dtos;

public class BatchOpenUserInput
{
    public List<Guid> CommonUserIds { get; set; } = new();
}
