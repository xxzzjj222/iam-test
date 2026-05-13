namespace LXT.IAM.Api.Bll.Services.Dashboard.Dtos;

public class GetTrendInput
{
    public string? AppCode { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
