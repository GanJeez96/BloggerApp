using System.Data;
using BloggerApp.Tests.Unit.Infrastructure.Helpers;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Dapper;
using Moq;

namespace BloggerApp.Tests.Unit.Infrastructure.Persistence.Tests;

public class AuthorRepositoryTests
{
    private readonly Mock<IMySqlConnectionFactory> _mockConnectionFactory;
    private readonly Mock<IDapperExecutor> _mockDapperExecutor;
    private readonly Mock<IDbConnection> _mockDbConnection;
    private readonly AuthorRepository _sut;
    
    public AuthorRepositoryTests()
    {
        _mockConnectionFactory = new Mock<IMySqlConnectionFactory>();
        _mockDapperExecutor = new Mock<IDapperExecutor>();
        _mockDbConnection = new Mock<IDbConnection>();

        _sut = new AuthorRepository(_mockConnectionFactory.Object, _mockDapperExecutor.Object);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldUseCorrectSql()
    {
        var id = 123L;
        var expectedAuthor = new Author() { Id = id, Name= "Test-Name", Surname = "Test Surname" };
        var sql = @"SELECT Id, Name, Surname FROM Authors WHERE Id = @Id";
        
        _mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(_mockDbConnection.Object);
        _mockDapperExecutor.Setup(d => d.QuerySingleOrDefaultAsync<Author>(
            _mockDbConnection.Object,
            sql,
            It.IsAny<object>()
        )).ReturnsAsync(expectedAuthor);

        var result = await _sut.GetByIdAsync(id);

        Assert.Equal(expectedAuthor, result);
        
        _mockDapperExecutor.Verify(d => d.QuerySingleOrDefaultAsync<Author>(
            _mockDbConnection.Object,
            sql,
            It.Is<object>(p => TestHelpers.HasParameter(p, "Id", expectedAuthor.Id))
        ), Times.Once);
    }
}