using Domain.Entities;

namespace Domain.Repositories;

public interface IAuthorRepository
{
    Task<long> AddAsync(Author author);
    Task<Author> GetByIdAsync(long id);
}