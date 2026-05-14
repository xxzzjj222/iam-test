namespace LXT.IAM.Api.Common.Models;

/// <summary>
/// 分页结果
/// </summary>
public class PagedList<T>
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public int Total { get; set; }

    public List<T> Items { get; set; } = new();
}
