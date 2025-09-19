using System.Data;
using Dapper;

namespace Infrastructure.Persistence.Dapper;

public class DapperExecutor : IDapperExecutor
{
    public Task<T?> QuerySingleOrDefaultAsync<T>(IDbConnection connection, string sql, object? param = null)
        => connection.QuerySingleOrDefaultAsync<T>(sql, param);

    public Task<T> ExecuteScalarAsync<T>(IDbConnection connection, string sql, object? param = null)
        => connection.ExecuteScalarAsync<T>(sql, param);
}