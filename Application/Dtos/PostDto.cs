namespace Application.Dtos;

public class PostDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public AuthorDto? Author { get; set; }
}