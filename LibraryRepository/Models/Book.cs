using System.ComponentModel.DataAnnotations;

namespace LibraryRepository.Models;

public class Book
{
    [Key]
    public Guid BookId{get; set;} = Guid.NewGuid();

    [Required]
    public string ISBN {get; set;}

    public Guid? AuthorId {get; set;}

    public Author? Author {get; set;}

    [Required]
    public string Title {get; set;}
    public string? Genre{get; set;}
    public string? Description{get; set;}
    public List<BookPictures>? bookPictures { get; set; }
   
}