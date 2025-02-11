using AutoMapper;
using Library.ViewModels;
using LibraryRepository.Models;

public class BookPictureProfile : Profile 
{
    public BookPictureProfile() 
    {
        CreateMap<BookPictures, BookPictureViewModel>();
        CreateMap<BookViewModel, BookPictures>().ForMember(x => x.Picture, opt => opt.MapFrom(bvm => bvm.BookPicture));
        // TODO: update the mapping
        CreateMap<BookPictureViewModel, BookPictures>();
    }
}