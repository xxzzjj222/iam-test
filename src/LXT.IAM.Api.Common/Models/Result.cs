namespace LXT.IAM.Api.Common.Models;

public class Result
{
    public Result()
    {
    }

    public Result(bool succeed)
    {
        Succeed = succeed;
    }

    public Result(string error, bool succeed = false)
    {
        Error = error;
        Succeed = succeed;
    }

    public bool Succeed { get; set; }

    public string Error { get; set; } = string.Empty;
}

public class Result<T> : Result
{
    public Result(T data)
    {
        Data = data;
        Succeed = true;
    }

    public Result(string error, bool succeed = false) : base(error, succeed)
    {
    }

    public T? Data { get; set; }
}
