using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;
using System.Data.Common;

namespace LXT.IAM.Api.Common.Orms.Dapper;

public class DapperProvider : IDapperProvider
{
    private readonly string _connectionString;

    public DapperProvider(IConfiguration configuration)
    {
        _connectionString = configuration["Db:IAMDb:ConnStr"] ?? string.Empty;
    }

    public async Task<T?> QueryAsync<T>(string sql, object? param = null)
    {
        await using var conn = new MySqlConnection(_connectionString);
        return await conn.QueryFirstOrDefaultAsync<T>(sql, param).ConfigureAwait(false);
    }

    public async Task<List<T>> QueryListAsync<T>(string sql, object? param = null)
    {
        await using var conn = new MySqlConnection(_connectionString);
        var result = await conn.QueryAsync<T>(sql, param).ConfigureAwait(false);
        return result.ToList();
    }

    public async Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? trans = null, IDbConnection? sourceConn = null)
    {
        if (sourceConn != null)
        {
            return await sourceConn.ExecuteAsync(sql, param, trans).ConfigureAwait(false);
        }

        await using var conn = new MySqlConnection(_connectionString);
        return await conn.ExecuteAsync(sql, param, trans).ConfigureAwait(false);
    }

    public async ValueTask<string> ExecuteTransactionAsync(Func<DbConnection, DbTransaction, Task> func)
    {
        var errorMsg = string.Empty;
        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync().ConfigureAwait(false);
        await using var trans = await conn.BeginTransactionAsync().ConfigureAwait(false);

        try
        {
            await func(conn, trans).ConfigureAwait(false);
            await trans.CommitAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            errorMsg = ex.ToString();
            await trans.RollbackAsync().ConfigureAwait(false);
        }

        return errorMsg;
    }
}
