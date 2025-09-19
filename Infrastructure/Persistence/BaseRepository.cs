using Infrastructure.Persistence.Dapper;

namespace Infrastructure.Persistence;

public abstract class BaseRepository(IMySqlConnectionFactory connectionFactory, IDapperExecutor dapper)
{
    protected async Task<T?> QuerySingleOrDefaultAsync<T>(string queryDescription, string sql, object param = null)
    {
        try
        {
            using var connection = connectionFactory.CreateConnection();
            return await dapper.QuerySingleOrDefaultAsync<T>(connection, sql, param);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ërror while executing query {queryDescription}: {e.Message}");
            throw;
        }
    }
    
    protected async Task<T> ExecuteScalarAsync<T>(string queryDescription, string sql, object param = null)
    {
        try
        {
            using var connection = connectionFactory.CreateConnection();
            return await dapper.ExecuteScalarAsync<T>(connection, sql, param);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ërror while executing query {queryDescription}: {e.Message}");
            throw;
        }
    }
}