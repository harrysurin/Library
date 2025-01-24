

namespace LibraryRepository.Models;

public class Book
{
    public Guid BookId{get; set;} = Guid.NewGuid();

    public required string ISBN {get; set;}

    public Guid? AuthorId {get; set;}

    public Author? Author {get; set;}
    
    public required string Title {get; set;}
    public string? Genre{get; set;}
    public string? Description{get; set;}
    public List<BookPictures>? bookPictures { get; set; }
   
}