namespace LXT.IAM.Api.Bll.Services.OpenUser.Dtos;

public class BatchOpenUserInput
{
    public List<Guid> UserIds { get; set; } = new();
}

