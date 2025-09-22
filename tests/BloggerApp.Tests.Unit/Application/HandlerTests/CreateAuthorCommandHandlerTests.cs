using Application.Commands;
using Application.Handlers;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Moq;

namespace BloggerApp.Tests.Unit.Application.HandlerTests;

public class CreateAuthorCommandHandlerTests
{
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly CreateAuthorCommandHandler _handler;

    public CreateAuthorCommandHandlerTests()
    {
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _handler = new CreateAuthorCommandHandler(_authorRepositoryMock.Object);
    }
    
    [Fact]
    public async Task Handle_WithValidRequest_ShouldCreateAuthor()
    {
        var request = new CreateAuthorCommand() { Name = "Test-Name", Surname =  "Test-Surname" };
        var newAuthorId = 5;
        
        _authorRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Author>()))
            .ReturnsAsync(newAuthorId);
        
        var result = await _handler.Handle(request, CancellationToken.None );

        result.Should().Be(newAuthorId);
        
        _authorRepositoryMock.Verify(r => r.AddAsync(It.Is<Author>(_ => 
            _.Name == request.Name 
            && _.Surname == request.Surname 
        )), Times.Once);
    }
}