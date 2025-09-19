using Domain.Entities;

namespace Domain.Repositories;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(long id);
    Task<long> AddAsync(Post post);
}