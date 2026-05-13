namespace LXT.IAM.Api.Bll.Migration.Dtos;

public class MigrateVersionInfoDto
{
    public string Version { get; set; } = string.Empty;

    public int VersionNum { get; set; }

    public string VersionFilePath { get; set; } = string.Empty;
}
