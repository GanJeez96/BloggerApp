using System.Data;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Dapper;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace BloggerApp.Tests.Unit.Infrastructure.Persistence.Tests;

public class BaseRepositoryTest
{
    private readonly Mock<IMySqlConnectionFactory>  _mockConnectionFactory;
    private readonly Mock<IDbConnection>  _mockDbConnection;
    private readonly Mock<IDapperExecutor> _mockDapperExecutor;
    private readonly TestRepository _sut;

    public BaseRepositoryTest()
    {
        _mockConnectionFactory = new Mock<IMySqlConnectionFactory>();
        _mockDbConnection  = new Mock<IDbConnection>();
        _mockDapperExecutor = new Mock<IDapperExecutor>();

        _sut = new TestRepository(_mockConnectionFactory.Object, _mockDapperExecutor.Object);
    }
    
    [Fact]
    public async Task QuerySingleOrDefaultAsync_ReturnsExpectedResult()
    {
        var sql = "SELECT 1";
        var parameters = new { };
        var expected = 1;
        
        _mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(_mockDbConnection.Object);
        
        _mockDapperExecutor.Setup(d => d.QuerySingleOrDefaultAsync<int?>(
                _mockDbConnection.Object, sql, parameters))
            .ReturnsAsync(expected);

        var result = await _sut.TestQuerySingleOrDefault(sql,parameters);

        Assert.Equal(expected, result);
        
        _mockDapperExecutor.Verify(d => d.QuerySingleOrDefaultAsync<int?>(
            _mockDbConnection.Object, sql, parameters), Times.Once);
    }
    
    [Fact]
    public async Task QuerySingleOrDefaultAsync_WhenError_ShouldThrowException()
    {
        var sql = "SELECT 1";
        var parameters = new { };
        var expectedException = new Exception("Database Error");
        
        _mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(_mockDbConnection.Object);
        
        _mockDapperExecutor.Setup(d => d.QuerySingleOrDefaultAsync<int?>(
                _mockDbConnection.Object, sql, parameters)).ThrowsAsync(expectedException);

        var result = async () => await _sut.TestQuerySingleOrDefault(sql,parameters);

        await result.Should().ThrowAsync<Exception>().WithMessage(expectedException.Message);
        
        _mockDapperExecutor.Verify(d => d.QuerySingleOrDefaultAsync<int?>(
            _mockDbConnection.Object, sql, parameters), Times.Once);
    }

    [Fact]
    public async Task ExecuteScalarAsync_ReturnsExpectedResult()
    {
        var sql = "SELECT COUNT(*)";
        var parameters = new { };
        var expected = 1;

        _mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(_mockDbConnection.Object);
        _mockDapperExecutor.Setup(d => d.ExecuteScalarAsync<int>(
                _mockDbConnection.Object, sql, parameters))
            .ReturnsAsync(expected);
        
        var result = await _sut.TestExecuteScalar(sql,parameters);
        
        Assert.Equal(expected, result);
        
        _mockDapperExecutor.Verify(d => d.ExecuteScalarAsync<int>(
            _mockDbConnection.Object, sql, parameters), Times.Once);
    }
    
    [Fact]
    public async Task ExecuteScalarAsync_WhenError_ShouldThrowException()
    {
        var sql = "SELECT COUNT(*)";
        var parameters = new { };
        var expectedException = new Exception("Database Error");

        _mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(_mockDbConnection.Object);
        
        _mockDapperExecutor.Setup(d => d.ExecuteScalarAsync<int>(
                _mockDbConnection.Object, sql, parameters)).ThrowsAsync(expectedException);
        
        var result =  async () => await _sut.TestExecuteScalar(sql,parameters);
        
        await result.Should().ThrowAsync<Exception>().WithMessage(expectedException.Message);
        
        _mockDapperExecutor.Verify(d => d.ExecuteScalarAsync<int>(
            _mockDbConnection.Object, sql, parameters), Times.Once);
    }
}