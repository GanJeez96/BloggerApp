using System.Data;
using MySql.Data.MySqlClient;

namespace Infrastructure.Persistence;

public class MySqlConnectionFactory(string connectionString) : IMySqlConnectionFactory
{
    public IDbConnection CreateConnection() => new MySqlConnection(connectionString);
}