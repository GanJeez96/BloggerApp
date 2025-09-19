using Domain.Entities;

namespace Domain.Repositories;

public interface IAuthorRepository
{
    Task<Author> GetByIdAsync(long id);
}