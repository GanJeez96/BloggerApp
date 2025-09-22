using Application.Commands;
using Application.Exceptions;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers;

public class CreatePostCommandHandler(IPostRepository postRepository, IAuthorRepository authorRepository) : IRequestHandler<CreatePostCommand, long>
{
    public async Task<long> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var author = await authorRepository.GetByIdAsync(request.AuthorId);

        if (author is null)
            throw new AuthorNotFoundException($"Author with id {request.AuthorId} does not exist");
        
        var post = new Post
        {
            AuthorId = request.AuthorId,
            Title = request.Title,
            Description = request.Description,
            Content = request.Content
        };
        
        var newPostId = await postRepository.AddAsync(post);
        
        return newPostId;
    }
}