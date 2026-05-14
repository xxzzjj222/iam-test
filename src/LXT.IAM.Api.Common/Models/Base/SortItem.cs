namespace LXT.IAM.Api.Common.Models.Base;

/// <summary>
/// 排序项
/// </summary>
public class SortItem
{
    public string Field { get; set; } = string.Empty;

    public string Order { get; set; } = "asc";
}
