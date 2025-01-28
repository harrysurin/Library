using AutoMapper;
using Library.ViewModels;
using LibraryRepository.Models;

public class BookProfile : Profile 
{
    public BookProfile() 
    {
        CreateMap<Book, BookViewModel>().ForMember(x => x.BookPicture, opt => opt.Ignore());
        CreateMap<BookViewModel, Book>();
        
    }
}