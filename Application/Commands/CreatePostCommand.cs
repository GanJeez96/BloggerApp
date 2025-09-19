using MediatR;

namespace Application.Commands;

public record CreatePostCommand(long AuthorId, string Title, string Description, string Content) : IRequest<long>;