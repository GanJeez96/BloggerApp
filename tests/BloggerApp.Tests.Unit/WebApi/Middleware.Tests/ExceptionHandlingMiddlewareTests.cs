using Application.Exceptions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using WebApi.Middleware;

namespace BloggerApp.Tests.Unit.WebApi.Middleware.Tests;

public class ExceptionHandlingMiddlewareTests
{
    [Fact]
        public async Task InvokeAsync_WhenNoException_ShouldCallNext()
        {
            var context = new DefaultHttpContext();
            var called = false;

            RequestDelegate next = ctx =>
            {
                called = true;
                return Task.CompletedTask;
            };

            var middleware = new ExceptionHandlingMiddleware(next);

            await middleware.InvokeAsync(context);

            called.Should().BeTrue();
            context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task InvokeAsync_WhenValidationException_ShouldReturn400()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            
            var validationException = new ValidationException([
                new ValidationFailure("Title", "Title is required")
            ]);

            var middleware = new ExceptionHandlingMiddleware(_ => throw validationException);

            await middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Response.Body);
            var body = await reader.ReadToEndAsync();

            body.Should().Contain("Title is required");
        }

        [Fact]
        public async Task InvokeAsync_WhenAuthorNotFoundException_ShouldReturn403()
        {
            var context = new DefaultHttpContext();
        
            var middleware = new ExceptionHandlingMiddleware(_ => throw new AuthorNotFoundException($"Author with id 123 does not exist"));
        
            await middleware.InvokeAsync(context);
        
            context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }
        
        [Fact]
        public async Task InvokeAsync_WhenGenericException_ShouldReturn500()
        {
            var context = new DefaultHttpContext();
        
            var middleware = new ExceptionHandlingMiddleware(_ => throw new InvalidOperationException("Something went wrong"));
        
            await middleware.InvokeAsync(context);
        
            context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
}