using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IRentHistoryServices
    {
        Task BookDistribution(Guid userId, Guid bookId);
        Task ReturnBook(Guid bookId, Guid userId);
        Task<bool> AccessToRent(Guid bookId);
        Task<List<RentHistory>> UserRentHistory(Guid userId);
        Task Delete(RentHistory rentHistory);
        Task<RentHistory?> GetByIdAsync(Guid id);
        PaginatedList<RentHistory> PaginatedList(int pageIndex, int pageSize, Guid userId);
    }
}