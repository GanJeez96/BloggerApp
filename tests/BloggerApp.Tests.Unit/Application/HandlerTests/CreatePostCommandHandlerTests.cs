using Application.Commands;
using Application.Exceptions;
using Application.Handlers;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Moq;

namespace BloggerApp.Tests.Unit.Application.HandlerTests;

public class CreatePostCommandHandlerTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly CreatePostCommandHandler _handler;

    public CreatePostCommandHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _handler = new CreatePostCommandHandler(_postRepositoryMock.Object, _authorRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenAuthorDoesNotExist_ShouldThrowAuthorNotFoundException()
    {
        var request = new CreatePostCommand { AuthorId = 100, Title = "Test-title", Content = "Test-content", Description  = "Test-description" };
        
        _authorRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(() => null);
        
        var action = async () => await _handler.Handle(request, CancellationToken.None );

        await action.Should().ThrowAsync<AuthorNotFoundException>().WithMessage($"Author with id {request.AuthorId} does not exist");
        
        _authorRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
        _postRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Post>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_WhenAuthorExists_ShouldCreatePost()
    {
        var request = new CreatePostCommand { AuthorId = 100, Title = "Test-title", Content = "Test-content", Description  = "Test-description" };
        var author = new Author{ Id = 10, Name = "John", Surname = "Doe"};
        var newPostId = 120;
        
        _authorRepositoryMock.Setup(r => r.GetByIdAsync(request.AuthorId))
            .ReturnsAsync(author);
        _postRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Post>()))
            .ReturnsAsync(newPostId);
        
        var result = await _handler.Handle(request, CancellationToken.None );

        result.Should().Be(newPostId);
        
        _authorRepositoryMock.Verify(r => r.GetByIdAsync(request.AuthorId), Times.Once);
        
        _postRepositoryMock.Verify(r => r.AddAsync(It.Is<Post>(_ => 
            _.AuthorId == request.AuthorId 
            && _.Title == request.Title 
            && _.Content == request.Content
            && _.Description == request.Description
            )), Times.Once);
    }
}