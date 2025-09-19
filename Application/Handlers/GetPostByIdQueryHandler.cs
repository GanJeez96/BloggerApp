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

        AuthorDto? authorDto = null;

        if(!request.IncludeAuthor)
            return new PostDto(post.Id, post.Title, post.Description, post.Content, authorDto);

        var author = await authorRepository.GetByIdAsync(post.AuthorId); 
        authorDto = new AuthorDto(author.Id, author.Name, author.Surname);

        return new PostDto(post.Id, post.Title, post.Description, post.Content, authorDto);
    }
}