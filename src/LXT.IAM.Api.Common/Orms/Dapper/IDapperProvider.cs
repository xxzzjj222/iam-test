using LXT.IAM.Api.Common.Intefaces.Base;
using System.Data;
using System.Data.Common;

namespace LXT.IAM.Api.Common.Orms.Dapper;

public interface IDapperProvider : IScopedDependency
{
    Task<T?> QueryAsync<T>(string sql, object? param = null);

    Task<List<T>> QueryListAsync<T>(string sql, object? param = null);

    Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? trans = null, IDbConnection? sourceConn = null);

    ValueTask<string> ExecuteTransactionAsync(Func<DbConnection, DbTransaction, Task> func);
}
