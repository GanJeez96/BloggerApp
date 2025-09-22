using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
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
    private string _apiKey = "TEST_KEY";
    
    protected HttpClient Client;
    protected TestDataManager TestDataManager;
    protected string ApiKeyHeader = "X-Api-Key";

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
        Client.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);

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
    
    #region Protected Methods

    protected StringContent CreateJsonHttpContent(object request)
    {
        var json = JsonSerializer.Serialize(request, JsonSerializerOptions);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    protected StringContent CreateXmlHttpContent(object request)
    {
        var serializer = new XmlSerializer(request.GetType());
        string xml;
        
        var settings = new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false),
            Indent = true,
            OmitXmlDeclaration = true
        };
        
        using (var memoryStream = new MemoryStream())
        using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
        {
            serializer.Serialize(xmlWriter, request);
            xmlWriter.Flush();

            xml = Encoding.UTF8.GetString(memoryStream.ToArray());
        }
        
        return new StringContent(xml, Encoding.UTF8, "application/xml");
    }

    #endregion
}