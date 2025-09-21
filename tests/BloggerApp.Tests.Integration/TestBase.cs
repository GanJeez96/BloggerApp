using System.Text.Json;
using BloggerApp.Tests.Integration.Common;
using BloggerApp.Tests.Integration.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BloggerApp.Tests.Integration;

[Collection("IntegrationTests")]
public abstract class TestBase : IAsyncLifetime
{
    private readonly DataStoreFixture _dataStoreFixture;
    private CustomWebApplicationFactory _factory;
    private IServiceScope _scope;
    
    protected HttpClient Client;
    protected TestDataManager TestDataManager;

    protected readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
    
    protected TestBase(DataStoreFixture dataStoreFixture, CustomWebApplicationFactory factory)
    {
        _dataStoreFixture = dataStoreFixture;
        _factory = factory;
        
        _factory.ConnectionString = _dataStoreFixture.ConnectionString;
    }

    public Task InitializeAsync()
    {
        Client = _factory.CreateClient();
        _scope = _factory.Services.CreateScope();
        
        TestDataManager = new TestDataManager(_scope.ServiceProvider);
        
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        Client?.Dispose();
        _scope?.Dispose();
        
        return Task.CompletedTask;
    }
}