using System.ComponentModel.DataAnnotations;

namespace LibraryRepository.Models;

public class Author 
{
    [Key]
    public Guid AuthorId {get; set;} = Guid.NewGuid();

    public string? Name {get; set;}

    public DateTime? DateOfBirth {get; set;}
}