using Application.Queries;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers;

public class GetPostByIdQueryHandler(IPostRepository postRepository) : IRequestHandler<GetPostByIdQuery, Post?>
{
    public async Task<Post?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        return await postRepository.GetByIdAsync(request.PostId);
    }
}