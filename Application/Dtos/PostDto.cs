namespace Application.Dtos;

public record PostDto(long Id,string Title, string Description, string Content, AuthorDto? Author);