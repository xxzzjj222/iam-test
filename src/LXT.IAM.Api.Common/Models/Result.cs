namespace LXT.IAM.Api.Common.Models;

/// <summary>
/// 通用结果
/// </summary>
public class Result
{
    /// <summary>
    /// 构造
    /// </summary>
    public Result()
    {
    }

    /// <summary>
    /// 构造
    /// </summary>
    public Result(bool succeed)
    {
        Succeed = succeed;
    }

    /// <summary>
    /// 构造
    /// </summary>
    public Result(string error, bool succeed = false)
    {
        Error = error;
        Succeed = succeed;
    }

    public bool Succeed { get; set; }

    public string Error { get; set; } = string.Empty;
}

/// <summary>
/// 泛型结果
/// </summary>
public class Result<T> : Result
{
    /// <summary>
    /// 构造
    /// </summary>
    public Result(T data)
    {
        Data = data;
        Succeed = true;
    }

    /// <summary>
    /// 构造
    /// </summary>
    public Result(string error, bool succeed = false) : base(error, succeed)
    {
    }

    public T? Data { get; set; }
}
