using LXT.IAM.Api.Common.Models.Base;

namespace LXT.IAM.Api.Bll.Services.User.Dtos;

public class GetUserPagedListInput : PaginationParams
{
    public string? Keyword { get; set; }
    public int? Status { get; set; }
    public string? AppCode { get; set; }
    public string? RegisterAppCode { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}
