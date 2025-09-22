using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using Application.Commands;
using Application.Dtos;
using BloggerApp.Tests.Integration.Fixtures;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace BloggerApp.Tests.Integration.Controllers;

public class PostControllerTests(DataStoreFixture dataStoreFixture, CustomWebApplicationFactory factory)
    : TestBase(dataStoreFixture, factory)
{
    #region Create post tests
    
    [Fact]
    public async Task Create_WithInvalidApiKey_ShouldReturn401Unauthorized()
    {
        var apiPath = "/posts";

        var request = new
        {
            AuthorId = 125,
            Title = "Test Title",
            Description = "Test Description",
            Content = "Test Content"
        };

        var content = CreateJsonHttpContent(request);

        Client.DefaultRequestHeaders.Remove(ApiKeyHeader);
        
        var response = await Client.PostAsync(apiPath, content);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Create_WithValidRequest_ShouldReturn200Ok_Json()
    {
        var apiPath = "/posts";
        var authorId = await TestDataManager.SeedAuthorData(new Author()
        {
            Name = "Test Author",
            Surname = "Integration"
        });

        var request = new
        {
            AuthorId = authorId,
            Title = "Test Title",
            Description = "Test Description",
            Content = "Test Content"
        };

        var content = CreateJsonHttpContent(request);
        
        var response = await Client.PostAsync(apiPath, content);
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task Create_WithValidRequest_ShouldReturn200Ok_Xml()
    {
        var apiPath = "/posts";
        var authorId = await TestDataManager.SeedAuthorData(new Author()
        {
            Name = "Test Author",
            Surname = "Integration"
        });

        var request = new CreatePostCommand
        {
            AuthorId = authorId,
            Title = "Test Title",
            Description = "Test Description",
            Content = "Test Content"
        };

        var content = CreateXmlHttpContent(request);
        
        var response = await Client.PostAsync(apiPath, content);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Create_WithInvalidRequestBody_ShouldReturn400BadRequest()
    {
        var apiPath = "/posts";
        
        var request = new
        {
            AuthorId = 0,
            Title = "  ",
            Description = new string('a', 501),
            Content = ""
        };

        var json = JsonSerializer.Serialize(request, JsonSerializerOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await Client.PostAsync(apiPath, content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    #endregion

    #region GetById Tests
    
    [Fact]
    public async Task GetById_WithInvalidApiKey_ShouldReturn401Unauthorized()
    {
        var postId = 1;
        var includeAuthor = false;
        var apiPath = $"/posts/{postId}?includeAuthor={includeAuthor}";

        Client.DefaultRequestHeaders.Remove(ApiKeyHeader);
        
        var response = await Client.GetAsync(apiPath);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetById_WhenIdIsInvalid_ShouldReturn400badRequest()
    {
        var postId = 0;
        var includeAuthor = false;
        var apiPath = $"/posts/{postId}?includeAuthor={includeAuthor}";
        
        var response = await Client.GetAsync(apiPath);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetById_WhenPostDoesNotExists_ShouldReturn404NotFound()
    {
        var postId = 5;
        var includeAuthor = false;
        var apiPath = $"/posts/{postId}?includeAuthor={includeAuthor}";
        
        var response = await Client.GetAsync(apiPath);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetById_WhenIncludeAuthorIsFalse_ShouldReturn200OkWithPost_Json()
    {
        var includeAuthor = false;

        var existingAuthor = new Author()
        {
            Name = "Test Author",
            Surname = "Integration"
        };
        
        var authorId = await TestDataManager.SeedAuthorData(existingAuthor);

        var existingPost = new Post()
        {
            AuthorId = authorId,
            Title = "Test Title",
            Description = "Test Description",
            Content = "Test Content"
        };

        var postId = await TestDataManager.SeedPostData(existingPost);
        
        var apiPath = $"/posts/{postId}?includeAuthor={includeAuthor}";
        
        var response = await Client.GetAsync(apiPath);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadFromJsonAsync<PostDto>(JsonSerializerOptions);
        
        responseContent?.Id.Should().Be(postId);
        responseContent?.Title.Should().Be(existingPost.Title);
        responseContent?.Description.Should().Be(existingPost.Description);
        responseContent?.Content.Should().Be(existingPost.Content);
        responseContent?.Author.Should().BeNull();
    }
    
    [Fact]
    public async Task GetById_WhenIncludeAuthorIsFalse_ShouldReturn200OkWithPost_Xml()
    {
        var includeAuthor = false;

        var existingAuthor = new Author()
        {
            Name = "Test Author",
            Surname = "Integration"
        };
        
        var authorId = await TestDataManager.SeedAuthorData(existingAuthor);

        var existingPost = new Post()
        {
            AuthorId = authorId,
            Title = "Test Title",
            Description = "Test Description",
            Content = "Test Content"
        };

        var postId = await TestDataManager.SeedPostData(existingPost);
        
        var apiPath = $"/posts/{postId}?includeAuthor={includeAuthor}";
        
        var request = new HttpRequestMessage(HttpMethod.Get, apiPath);
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
        
        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        var stream = await response.Content.ReadAsStreamAsync();
        var serializer = new XmlSerializer(typeof(PostDto));
        var responseContent = (PostDto?)serializer.Deserialize(stream);
        
        responseContent?.Id.Should().Be(postId);
        responseContent?.Title.Should().Be(existingPost.Title);
        responseContent?.Description.Should().Be(existingPost.Description);
        responseContent?.Content.Should().Be(existingPost.Content);
        responseContent?.Author.Should().BeNull();
    }
    
    [Fact]
    public async Task GetById_WhenIncludeAuthorIsTrue_ShouldReturn200OkWithPostAndAuthor_Json()
    {
        var includeAuthor = true;

        var existingAuthor = new Author()
        {
            Name = "Test Author",
            Surname = "Integration"
        };
        
        var authorId = await TestDataManager.SeedAuthorData(existingAuthor);

        var existingPost = new Post()
        {
            AuthorId = authorId,
            Title = "Test Title",
            Description = "Test Description",
            Content = "Test Content"
        };

        var postId = await TestDataManager.SeedPostData(existingPost);
        
        var apiPath = $"/posts/{postId}?includeAuthor={includeAuthor}";
        
        var response = await Client.GetAsync(apiPath);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadFromJsonAsync<PostDto>(JsonSerializerOptions);
        
        responseContent?.Id.Should().Be(postId);
        responseContent?.Title.Should().Be(existingPost.Title);
        responseContent?.Description.Should().Be(existingPost.Description);
        responseContent?.Content.Should().Be(existingPost.Content);
        responseContent?.Author?.Name.Should().Be(existingAuthor.Name);
        responseContent?.Author?.Surname.Should().Be(existingAuthor.Surname);
    }
    
    [Fact]
    public async Task GetById_WhenIncludeAuthorIsTrueAnd_ShouldReturn200OkWithPostAndAuthor_Xml()
    {
        var includeAuthor = true;

        var existingAuthor = new Author()
        {
            Name = "Test Author",
            Surname = "Integration"
        };
        
        var authorId = await TestDataManager.SeedAuthorData(existingAuthor);

        var existingPost = new Post()
        {
            AuthorId = authorId,
            Title = "Test Title",
            Description = "Test Description",
            Content = "Test Content"
        };

        var postId = await TestDataManager.SeedPostData(existingPost);
        
        var apiPath = $"/posts/{postId}?includeAuthor={includeAuthor}";
        
        var request = new HttpRequestMessage(HttpMethod.Get, apiPath);
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
        
        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        var stream = await response.Content.ReadAsStreamAsync();
        var serializer = new XmlSerializer(typeof(PostDto));
        var responseContent = (PostDto?)serializer.Deserialize(stream);
        
        responseContent?.Id.Should().Be(postId);
        responseContent?.Title.Should().Be(existingPost.Title);
        responseContent?.Description.Should().Be(existingPost.Description);
        responseContent?.Content.Should().Be(existingPost.Content);
        responseContent?.Author?.Name.Should().Be(existingAuthor.Name);
        responseContent?.Author?.Surname.Should().Be(existingAuthor.Surname);
    }
    
    #endregion
}