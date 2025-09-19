using Infrastructure.Persistence;
using Infrastructure.Persistence.Dapper;

namespace BloggerApp.Tests.Unit.Infrastructure.Persistence.Tests;

public class TestRepository(IMySqlConnectionFactory factory, IDapperExecutor dapper) : BaseRepository(factory, dapper)
{
    public Task<int?> TestQuerySingleOrDefault(string sql, object param = null) =>
        QuerySingleOrDefaultAsync<int?>("Test query", sql, param);

    public Task<int> TestExecuteScalar(string sql, object param = null) =>
        ExecuteScalarAsync<int>("Test scalar", sql, param);
}