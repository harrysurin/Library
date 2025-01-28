using AutoMapper;
using Library.ViewModels;
using LibraryRepository.Models;

public class AuthorProfile : Profile 
{
    public AuthorProfile() 
    {
        CreateMap<Author, AuthorViewModel>();
        CreateMap<AuthorViewModel, Author>();
    }
}