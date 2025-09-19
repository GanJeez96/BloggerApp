using System.Data;

namespace Infrastructure.Persistence;

public interface IMySqlConnectionFactory
{
    IDbConnection CreateConnection();
}