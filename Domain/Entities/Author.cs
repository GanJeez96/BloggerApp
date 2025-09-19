namespace Domain.Entities;

public class Author
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Surname { get; init; } =  string.Empty;
}