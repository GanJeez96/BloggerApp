using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BloggerApp.Tests.Integration.Common;

public class TestDataManager(IServiceProvider services)
{
    private readonly IAuthorRepository  _authorRepository = services.GetRequiredService<IAuthorRepository>();
    private readonly IPostRepository  _postRepository = services.GetRequiredService<IPostRepository>();

    public async Task<long> SeedAuthorData(Author newAuthor)
    {
        var newAuthorId = await _authorRepository.AddAsync(newAuthor);
        return newAuthorId;
    }
    
    public async Task<long> SeedPostData(Post newPost)
    {
        var newPostId = await _postRepository.AddAsync(newPost);
        return newPostId;
    }
}