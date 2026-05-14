using LXT.IAM.Api.Common.Intefaces.Base;
using System.Data;
using System.Data.Common;

namespace LXT.IAM.Api.Common.Orms.Dapper;

/// <summary>
/// Dapper 访问接口
/// </summary>
public interface IDapperProvider : IScopedDependency
{
    /// <summary>
    /// 查询单条记录
    /// </summary>
    Task<T?> QueryAsync<T>(string sql, object? param = null);

    /// <summary>
    /// 查询列表
    /// </summary>
    Task<List<T>> QueryListAsync<T>(string sql, object? param = null);

    /// <summary>
    /// 执行 SQL
    /// </summary>
    Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? trans = null, IDbConnection? sourceConn = null);

    /// <summary>
    /// 执行事务
    /// </summary>
    ValueTask<string> ExecuteTransactionAsync(Func<DbConnection, DbTransaction, Task> func);
}
