namespace LibraryRepository.Models;

public class Author 
{
    public Guid AuthorId {get; set;} = Guid.NewGuid();

    public required string FirstName {get; set;}

    public string? LastName {get; set;}

    public DateTime? DateOfBirth {get; set;}

    public string? Country {get; set; }
}