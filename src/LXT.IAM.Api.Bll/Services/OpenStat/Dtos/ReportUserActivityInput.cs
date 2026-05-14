namespace LXT.IAM.Api.Bll.Services.OpenStat.Dtos;

public class ReportUserActivityInput
{
    public Guid UserId { get; set; }

    public string AppCode { get; set; } = string.Empty;

    public DateTime? ActiveTime { get; set; }
}

