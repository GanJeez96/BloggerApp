using Application.Commands;
using Application.Validators;
using FluentAssertions;

namespace BloggerApp.Tests.Unit.Application.Validators;

public class CreateAuthorCommandValidatorTests
{
    private readonly CreateAuthorCommandValidator _validator = new();
    
    [Fact]
    public void Validator_WhenCommandIsValid_ShouldNotReturnError()
    {
        var command = new CreateAuthorCommand{ Name = "Test" , Surname =  "Test"};

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void Validator_WhenNameIsEmpty_ShouldReturnError()
    {
        var command = new CreateAuthorCommand{ Name = string.Empty , Surname =  "Test"};
        
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validator_WhenNameIsTooLong_ShouldReturnError()
    {
        var longName = new string('a', 51);
        var command = new CreateAuthorCommand{ Name = longName , Surname =  "Test"};
        
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }
    
    [Fact]
    public void Validator_WhenSurnameIsEmpty_ShouldReturnError()
    {
        var command = new CreateAuthorCommand{ Name = "Test", Surname = string.Empty};
        
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        
        Assert.Contains(result.Errors, e => e.PropertyName == "Surname");
    }

    [Fact]
    public void Validator_WhenSurnameIsTooLong_ShouldReturnError()
    {
        var longSurname = new string('a', 51);
        var command = new CreateAuthorCommand{ Name = "Test" , Surname =  longSurname};
        
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        Assert.Contains(result.Errors, e => e.PropertyName == "Surname");
    }
}