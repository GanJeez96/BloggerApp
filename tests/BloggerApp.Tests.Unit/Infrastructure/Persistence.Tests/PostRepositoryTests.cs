using System.Data;
using BloggerApp.Tests.Unit.Infrastructure.Helpers;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Dapper;
using Moq;

namespace BloggerApp.Tests.Unit.Infrastructure.Persistence.Tests;

public class PostRepositoryTests
{
    private readonly Mock<IMySqlConnectionFactory> _mockConnectionFactory;
    private readonly Mock<IDapperExecutor> _mockDapperExecutor;
    private readonly Mock<IDbConnection> _mockDbConnection;
    private readonly PostRepository _sut;
    
    public PostRepositoryTests()
    {
        _mockConnectionFactory = new Mock<IMySqlConnectionFactory>();
        _mockDapperExecutor = new Mock<IDapperExecutor>();
        _mockDbConnection = new Mock<IDbConnection>();

        _sut = new PostRepository(_mockConnectionFactory.Object, _mockDapperExecutor.Object);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldUseCorrectSql()
    {
        var id = 123L;
        var expectedPost = new Post { Id = id, AuthorId = 1, Title = "Test-title", Content = "Test Content", Description = "Test description" };
        var sql = @"SELECT Id, AuthorId, Title, Description, Content FROM Posts WHERE Id = @Id";
        
        _mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(_mockDbConnection.Object);
        _mockDapperExecutor.Setup(d => d.QuerySingleOrDefaultAsync<Post>(
                _mockDbConnection.Object,
                sql,
                It.IsAny<object>()
                )).ReturnsAsync(expectedPost);

        var result = await _sut.GetByIdAsync(id);

        Assert.Equal(expectedPost, result);
        
        _mockDapperExecutor.Verify(d => d.QuerySingleOrDefaultAsync<Post>(
            _mockDbConnection.Object,
            sql,
            It.Is<object>(p => TestHelpers.HasParameter(p, "Id", id))
        ), Times.Once);
    }
    
    [Fact]
    public async Task AddAsync_ShouldUseCorrectSql()
    {
        var expectedNewPostId = 123L;
        var newPostRequest = new Post {AuthorId = 1, Title = "Test-title", Content = "Test Content", Description = "Test description" };
        var sql = @"INSERT INTO Posts (AuthorId, Title, Description, Content) VALUES (@AuthorId, @Title, @Description, @Content); SELECT LAST_INSERT_ID();";
        
        _mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(_mockDbConnection.Object);
        _mockDapperExecutor.Setup(d => d.ExecuteScalarAsync<long>(
            _mockDbConnection.Object,
            sql,
            It.IsAny<object>()
        )).ReturnsAsync(expectedNewPostId);

        var result = await _sut.AddAsync(newPostRequest);

        Assert.Equal(expectedNewPostId, result);
        
        _mockDapperExecutor.Verify(d => d.ExecuteScalarAsync<long>(
            _mockDbConnection.Object,
            sql,
            It.Is<object>(p => TestHelpers.HasParameter(p, "AuthorId", newPostRequest.AuthorId)
            && TestHelpers.HasParameter(p, "Title", newPostRequest.Title)
            && TestHelpers.HasParameter(p, "Description", newPostRequest.Description)
            && TestHelpers.HasParameter(p, "Content", newPostRequest.Content)
            )
        ), Times.Once);
    }
}