using Dapper;

namespace Infrastructure.Persistence;

public abstract class BaseRepository(MySqlConnectionFactory connectionFactory)
{
    protected async Task<T?> QuerySingleOrDefaultAsync<T>(string queryDescription, string sql, object param = null)
    {
        try
        {
            using var connection = connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
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
            return await connection.ExecuteScalarAsync<T>(sql, param);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ërror while executing query {queryDescription}: {e.Message}");
            throw;
        }
    }
}