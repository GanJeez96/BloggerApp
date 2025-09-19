using Domain.Entities;

namespace Domain.Repositories;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(long id);
    Task<int> AddAsync(Post post);
}