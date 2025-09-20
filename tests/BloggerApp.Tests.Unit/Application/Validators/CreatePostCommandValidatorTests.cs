using Application.Commands;
using Application.Validators;
using FluentAssertions;

namespace BloggerApp.Tests.Unit.Application.Validators;

public class CreatePostCommandValidatorTests
{
    private readonly CreatePostCommandValidator _validator = new();

    [Fact]
    public void Validator_WhenCommandIsValid_ShouldNotReturnError()
    {
        var command = new CreatePostCommand{ AuthorId = 1, Title = "Valid Title", Description = "Valid Description", Content = "Valid Content"};

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validator_WhenAuthorIdIsEmpty_ShouldReturnError()
    {
        var command = new CreatePostCommand{ AuthorId = 0, Title = "Valid Title", Description = "Valid Description", Content = "Valid Content"};

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        Assert.Contains(result.Errors, e => e.PropertyName == "AuthorId");
    }

    [Fact]
    public void Validator_WhenTitleIsEmpty_ShouldReturnError()
    {
        var command = new CreatePostCommand{ AuthorId = 1, Title = string.Empty, Description = "Valid Description", Content = "Valid Content"};

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public void Validator_WhenTitleIsTooLong_ShouldReturnError()
    {
        var longTitle = new string('a', 201);
        var command = new CreatePostCommand{ AuthorId = 1, Title = longTitle, Description = "Valid Description", Content = "Valid Content"};

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public void Validator_WhenDescriptionIsTooLong_ShouldReturnError()
    {
        var longDesc = new string('a', 501);
        var command = new CreatePostCommand{ AuthorId = 1, Title = "Valid Title", Description = longDesc, Content = "Valid Content"};

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        Assert.Contains(result.Errors, e => e.PropertyName == "Description");
    }

    [Fact]
    public void Validator_WhenContentIsEmpty_ShouldReturnError()
    {
        var command = new CreatePostCommand{ AuthorId = 1, Title = "Valid Title", Description = "Valid Description", Content = string.Empty};

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Content");
    }
}