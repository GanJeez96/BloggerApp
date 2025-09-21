using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Dapper;

namespace Infrastructure.Persistence;

public class AuthorRepository(IMySqlConnectionFactory connectionFactory, IDapperExecutor dapper) : BaseRepository(connectionFactory, dapper), IAuthorRepository
{
    public async Task<long> AddAsync(Author author)
    {
        var parameters = new
        {
            author.Name,
            author.Surname
        };

        var newAuthorId = await ExecuteScalarAsync<long>(nameof(_insertAuthorQuery), _insertAuthorQuery, parameters);
        return newAuthorId;
    }

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
    
    private readonly string _insertAuthorQuery = @"INSERT INTO Authors (Name, Surname) VALUES (@Name, @Surname); SELECT LAST_INSERT_ID();";

    private readonly string _getAuthorByIdQuery = @"SELECT Id, Name, Surname FROM Authors WHERE Id = @Id";
    
    #endregion
}