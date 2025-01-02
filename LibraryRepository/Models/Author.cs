using System.ComponentModel.DataAnnotations;

namespace LibraryRepository.Models;

public class Author 
{
    [Key]
    public Guid AuthorId {get; set;} = Guid.NewGuid();

    public string FirstName {get; set;}

    public string LastName {get; set;}

    public DateTime? DateOfBirth {get; set;}

    public string Country {get; set; }
}