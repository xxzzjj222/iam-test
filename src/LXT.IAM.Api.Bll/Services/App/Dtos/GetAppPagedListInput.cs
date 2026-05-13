using LXT.IAM.Api.Common.Models.Base;

namespace LXT.IAM.Api.Bll.Services.App.Dtos;

public class GetAppPagedListInput : PaginationParams
{
    public string? Keyword { get; set; }
    public int? Status { get; set; }
}
