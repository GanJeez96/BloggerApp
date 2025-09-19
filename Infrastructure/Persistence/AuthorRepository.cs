using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Dapper;

namespace Infrastructure.Persistence;

public class AuthorRepository(IMySqlConnectionFactory connectionFactory, IDapperExecutor dapper) : BaseRepository(connectionFactory, dapper), IAuthorRepository
{
    public async Task<Author> GetByIdAsync(long id)
    {
        var parameters = new
        {
            Id = id
        };

        var result = await QuerySingleOrDefaultAsync<Author>(nameof(_getAuthorByIdQuery), _getAuthorByIdQuery, parameters);
        return result;
    }
    
    #region Sql queries
    
    private readonly string _getAuthorByIdQuery = @"SELECT Id, Name, Surname FROM Authors WHERE Id = @Id";
    
    #endregion
}