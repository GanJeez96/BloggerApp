using Application.Dtos;
using Application.Queries;
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
    
    [Theory]
    [InlineData(0)]
    [InlineData(-97)]
    public async Task GetById_WhenPostIdIsInvalid_ShouldReturnBadRequest(long postId)
    {
        var result = await _controller.GetById(postId, includeAuthor: false);

        Assert.IsType<BadRequestObjectResult>(result);
        
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

        Assert.IsType<NotFoundResult>(result);
        
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetPostByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenPostExistsAndIncludeAuthorIsFalse_ShouldReturnOnlyPostWithOkResult()
    {
        var postId = 1;
        var expectedPost = new PostDto(postId, "Title", "Desc", "Content", null);
        var query = new GetPostByIdQuery(postId, false);

        _mediatorMock
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPost);

        var result = await _controller.GetById(postId, includeAuthor: false);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedPost = Assert.IsType<PostDto>(okResult.Value);
        
        Assert.Equal(expectedPost, returnedPost);
        
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetPostByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenPostExistsAndIncludeAuthorIsTrue_ShouldReturnPostAndAuthorWithOkResult()
    {
        var postId = 1;
        var includeAuthor = true;
        var query = new GetPostByIdQuery(postId, includeAuthor);
        var expectedPost = new PostDto(postId, "Title", "Desc", "Content", new AuthorDto(2, "John", "Doe"));

        _mediatorMock
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPost);

        var result = await _controller.GetById(postId, includeAuthor);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedPost = Assert.IsType<PostDto>(okResult.Value);
        
        Assert.Equal(expectedPost, returnedPost);
        
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetPostByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}