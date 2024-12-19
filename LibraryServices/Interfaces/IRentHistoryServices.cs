using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IRentHistoryServices
    {
        Task BookDistribution(Guid userId, Guid bookId);
        Task ReturnBook(Guid bookId, Guid userId);
        Task<bool> AccessToReturn(Guid bookId);
        Task<List<RentHistory>> UserRentHistory(Guid userId);
        Task Delete(RentHistory rentHistory);
    }
}