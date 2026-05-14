namespace LXT.IAM.Api.Common.Models;

/// <summary>
/// 长整型-字符串键值对
/// </summary>
public class LongStringKV
{
    public long Key { get; set; }

    public string Value { get; set; } = string.Empty;
}
