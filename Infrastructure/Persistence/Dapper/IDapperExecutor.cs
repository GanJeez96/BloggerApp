using System.Data;

namespace Infrastructure.Persistence.Dapper;

public interface IDapperExecutor
{
    Task<T?> QuerySingleOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null);
    Task<T> ExecuteScalarAsync<T>(IDbConnection connection, string sql, object param = null);
}