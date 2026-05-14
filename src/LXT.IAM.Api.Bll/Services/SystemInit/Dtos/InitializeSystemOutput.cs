namespace LXT.IAM.Api.Bll.Services.SystemInit.Dtos;

public class InitializeSystemOutput
{
    public Guid UserId { get; set; }

    public string AdminAccount { get; set; } = string.Empty;

    public string PlatformRoleCode { get; set; } = string.Empty;

    public bool Created { get; set; }
}

