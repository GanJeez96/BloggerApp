using Application.Commands;
using Application.Dtos;
using Application.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace BloggerApp.Tests.Unit.WebApi.Controller.Tests;

public class PostControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PostController _controller;

    public PostControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PostController(_mediatorMock.Object);
    }
    
    [Fact]
    public async Task Create_ReturnsOk_WithId()
    {
        var expectedNewPostId = 110;
        var command = new CreatePostCommand{ AuthorId = 1, Title = "Title", Description = "Desc", Content = "Content"};
        
        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedNewPostId);
        
        var result = await _controller.Create(command);
        
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(expectedNewPostId);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-97)]
    public async Task GetById_WhenPostIdIsInvalid_ShouldReturnBadRequest(long postId)
    {
        var result = await _controller.GetById(postId, includeAuthor: false);

        result.Should().BeOfType<BadRequestObjectResult>();
        
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetPostByIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetById_WhenPostDoesNotExist_ShouldReturnNotFound()
    {
        var postId = 1;
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetPostByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);
        
        var result = await _controller.GetById(postId, includeAuthor: false);

        result.Should().BeOfType<NotFoundResult>();
        
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetPostByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenPostExistsAndIncludeAuthorIsFalse_ShouldReturnOnlyPostWithOkResult()
    {
        var postId = 1;
        var expectedPost = new PostDto{ Id = postId, Title = "Title", Description = "Desc", Content = "Content", Author = null};
        var query = new GetPostByIdQuery(postId, false);

        _mediatorMock
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPost);

        var result = await _controller.GetById(postId, includeAuthor: false);
        
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(expectedPost);
        
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetPostByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenPostExistsAndIncludeAuthorIsTrue_ShouldReturnPostAndAuthorWithOkResult()
    {
        var postId = 1;
        var includeAuthor = true;
        var query = new GetPostByIdQuery(postId, includeAuthor);
        var expectedPost = new PostDto{ Id = postId, Title = "Title", Description = "Desc", Content = "Content", Author = null};

        _mediatorMock
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPost);

        var result = await _controller.GetById(postId, includeAuthor);
        
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(expectedPost);
        
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetPostByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}