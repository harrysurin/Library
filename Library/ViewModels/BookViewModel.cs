using LibraryRepository.Models;

namespace Library.ViewModels
{
    public class BookViewModel
    {
        public Guid BookId{get; set;} = Guid.NewGuid();

        public string ISBN {get; set;}

        public Guid? AuthorId {get; set;}
        public string Title {get; set;}
        public string? Genre{get; set;}
        public string? Description{get; set;}

        public IFormFile? BookPicture{get; set;}
    }
}