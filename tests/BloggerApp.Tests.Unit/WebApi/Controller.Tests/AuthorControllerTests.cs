using Application.Commands;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace BloggerApp.Tests.Unit.WebApi.Controller.Tests;

public class AuthorControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthorController _controller;

    public AuthorControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthorController(_mediatorMock.Object);
    }
    
    [Fact]
    public async Task Create_ReturnsOk_WithId()
    {
        var expectedNewAuthorId = 110;
        var command = new CreateAuthorCommand{ Name =  "Test Name", Surname = "Test Surname" };
        
        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedNewAuthorId);
        
        var result = await _controller.Create(command);
        
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(expectedNewAuthorId);
        
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }
}