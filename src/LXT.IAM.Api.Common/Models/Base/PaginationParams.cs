using Newtonsoft.Json;

namespace LXT.IAM.Api.Common.Models.Base;

public class PaginationParams
{
    [JsonProperty("pageIndex")]
    public int PageIndex { get; set; } = 1;

    [JsonProperty("pageSize")]
    public int PageSize { get; set; } = 10;

    public List<SortItem> Sorts { get; set; } = new();
}
