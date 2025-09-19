namespace Domain.Entities;

public class Post
{
    public long Id { get; init; }
    public long AuthorId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
}