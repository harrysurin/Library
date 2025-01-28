using AutoMapper;
using Library.ViewModels;
using LibraryRepository.Models;

public class BookPictureProfile : Profile 
{
    public BookPictureProfile() 
    {
        CreateMap<BookPictures, BookPictureViewModel>();
        CreateMap<BookPictureViewModel, BookPictures>();
       
    }
}