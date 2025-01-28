using AutoMapper;
using Library.ViewModels;
using LibraryRepository.Models;

public class RentHistoryProfile : Profile 
{
    public RentHistoryProfile() 
    {
        CreateMap<RentHistory, RentHistoryViewModel>();
        CreateMap<RentHistoryViewModel, RentHistory>();
        // CreateMap<List<RentHistory>, List<RentHistoryViewModel>>();
    }
}