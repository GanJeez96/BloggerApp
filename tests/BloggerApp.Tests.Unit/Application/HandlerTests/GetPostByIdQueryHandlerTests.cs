using Application.Dtos;
using Application.Handlers;
using Application.Queries;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Moq;

namespace BloggerApp.Tests.Unit.Application.HandlerTests;

public class GetPostByIdQueryHandlerTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly GetPostByIdQueryHandler _handler;

    public GetPostByIdQueryHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _handler = new GetPostByIdQueryHandler(_postRepositoryMock.Object, _authorRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenPostDoesNotExist_ShouldReturnNull()
    {
        _postRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(() => null);

        var query = new GetPostByIdQuery(1, false);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Null(result);
        
        _postRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
        _authorRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenIncludeAuthorIsFalse_ShouldReturnPostWithoutAuthor()
    {
        var post = new Post { Id = 1, AuthorId = 10, Title = "Test Title", Description = "Test Desc", Content = "Test Content"};
        
        _postRepositoryMock.Setup(r => r.GetByIdAsync(post.Id))
            .ReturnsAsync(post);
    
        var query = new GetPostByIdQuery(post.Id, false);
    
        var result = await _handler.Handle(query, CancellationToken.None);
    
        var expected = new PostDto{ Id = post.Id, Title = post.Title, Description = post.Description, Content = post.Content, Author = null};
        
        result.Should().BeEquivalentTo(expected);
        
        _postRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
        _authorRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_WhenIncludeAuthorIsTrue_ShouldReturnPostWithAuthor()
    {
        var post = new Post { Id = 1, AuthorId = 10, Title = "Test Title", Description = "Test Desc", Content = "Test Content"};
        var author = new Author{ Id = 10, Name = "John", Surname = "Doe"};
    
        _postRepositoryMock.Setup(r => r.GetByIdAsync(post.Id))
            .ReturnsAsync(post);
        _authorRepositoryMock.Setup(r => r.GetByIdAsync(post.AuthorId))
            .ReturnsAsync(author);
    
        var query = new GetPostByIdQuery(post.Id, true);
    
        var result = await _handler.Handle(query, CancellationToken.None);
    
        var expectedAuthorDto = new AuthorDto{Id = author.Id, Name = author.Name, Surname = author.Surname};
        var expected = new PostDto{ Id = post.Id, Title = post.Title, Description = post.Description, Content = post.Content, Author = expectedAuthorDto};
        
        result.Should().BeEquivalentTo(expected);
        
        _postRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
        _authorRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
    }

}