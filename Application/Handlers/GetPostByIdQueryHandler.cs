using Application.Dtos;
using Application.Queries;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers;

public class GetPostByIdQueryHandler(IPostRepository postRepository, IAuthorRepository authorRepository) : IRequestHandler<GetPostByIdQuery, PostDto?>
{
    public async Task<PostDto?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(request.PostId);

        if (post is null)
            return null;
        
        var postResult = new PostDto{ Id = post.Id, Title = post.Title, Description = post.Description, Content = post.Content};

        if (!request.IncludeAuthor)
            return postResult;

        var author = await authorRepository.GetByIdAsync(post.AuthorId); 
        postResult.Author = new AuthorDto{ Id = author.Id, Name = author.Name, Surname = author.Surname};

        return postResult;
    }
}