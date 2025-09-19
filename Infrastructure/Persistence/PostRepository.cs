using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Dapper;

namespace Infrastructure.Persistence;

public class PostRepository(IMySqlConnectionFactory connectionFactory, IDapperExecutor dapper) : BaseRepository(connectionFactory, dapper), IPostRepository
{
    public async Task<Post?> GetByIdAsync(long id)
    {
        var parameters = new
        {
            Id = id
        };

        var result = await QuerySingleOrDefaultAsync<Post>(nameof(_getPostByIdQuery), _getPostByIdQuery, parameters);
        return result;
    }

    public async Task<int> AddAsync(Post post)
    {
        var parameters = new
        {
            post.AuthorId,
            post.Title,
            post.Description,
            post.Content,
        };

        var newPostId = await ExecuteScalarAsync<int>(nameof(_insertPostQuery), _insertPostQuery, parameters);
        return newPostId;
    }

    #region Sql queries

    private readonly string _getPostByIdQuery = @"SELECT Id, AuthorId, Title, Description, Content FROM Posts WHERE Id = @Id";
    
    private readonly string _insertPostQuery = @"INSERT INTO Posts (AuthorId, Title, Description, Content) VALUES (@AuthorId, @Title, @Description, @Content); SELECT LAST_INSERT_ID();";

    #endregion
}