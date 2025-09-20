using MediatR;

namespace Application.Commands;

public class CreatePostCommand : IRequest<long>
{
    public long AuthorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}