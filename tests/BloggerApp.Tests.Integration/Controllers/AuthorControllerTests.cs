using System.Net;
using System.Text;
using System.Text.Json;
using Application.Commands;
using BloggerApp.Tests.Integration.Fixtures;
using FluentAssertions;
using Xunit;

namespace BloggerApp.Tests.Integration.Controllers;

public class AuthorControllerTests(DataStoreFixture dataStoreFixture, CustomWebApplicationFactory factory)
    : TestBase(dataStoreFixture, factory)
{
    [Fact]
    public async Task Create_WithInvalidApiKey_ShouldReturn401Unauthorized()
    {
        var apiPath = "/authors";

        var request = new
        {
            Name = "Test Author",
            Surname = "Integration"
        };

        var content = CreateJsonHttpContent(request);

        Client.DefaultRequestHeaders.Remove(ApiKeyHeader);
        var response = await Client.PostAsync(apiPath, content);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Create_WithValidRequest_ShouldReturn200Ok_Json()
    {
        var apiPath = "/authors";

        var request = new
        {
            Name = "Test Author",
            Surname = "Integration"
        };

        var content = CreateJsonHttpContent(request);
        
        var response = await Client.PostAsync(apiPath, content);
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task Create_WithValidRequest_ShouldReturn200Ok_Xml()
    {
        var apiPath = "/authors";

        var request = new CreateAuthorCommand
        {
            Name = "Test Author",
            Surname = "Integration"
        };

        var content = CreateXmlHttpContent(request);
        
        var response = await Client.PostAsync(apiPath, content);
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task Create_WithInvalidRequestBody_ShouldReturn400BadRequest()
    {
        var apiPath = "/authors";
        
        var request = new
        {
            Name = "  ",
            Surname = new string('a', 501)
        };

        var json = JsonSerializer.Serialize(request, JsonSerializerOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await Client.PostAsync(apiPath, content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}