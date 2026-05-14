namespace LXT.IAM.Api.Bll.Services.SystemInit.Dtos;

public class SystemInitStatusOutput
{
    public bool Initialized { get; set; }

    public bool HasSuperAdminRole { get; set; }

    public bool HasSuperAdminUser { get; set; }

    public int AppCount { get; set; }

    public int PlatformFunctionCount { get; set; }
}
