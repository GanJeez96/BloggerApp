using Application.Commands;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers;

public class CreatePostCommandHandler(IPostRepository postRepository) : IRequestHandler<CreatePostCommand, long>
{
    public async Task<long> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
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