using MediatR;

namespace Application.Commands;

public class CreateAuthorCommand : IRequest<long>
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } =  string.Empty;
}