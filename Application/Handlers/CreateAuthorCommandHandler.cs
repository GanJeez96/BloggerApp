using Application.Commands;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers;

public class CreateAuthorCommandHandler(IAuthorRepository authorRepository) : IRequestHandler<CreateAuthorCommand, long>
{
    public async Task<long> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = new Author
        {
            Name = request.Name,
            Surname = request.Surname,
        };
        
        var newAuthorId = await authorRepository.AddAsync(author);
        
        return newAuthorId;
    }
}