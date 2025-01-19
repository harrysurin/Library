using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IRentHistoryServices
    {
        Task<IEnumerable<RentHistory>> GetAllAsync();
        Task BookRent(Guid userId, Guid bookId);
        Task ReturnBook(Guid bookId, Guid userId);
        Task<bool> IsAvailableToRent(Guid bookId);
        Task<List<RentHistory>> GetUserRentHistory(Guid userId);
        Task Delete(RentHistory rentHistory);
        Task<RentHistory?> GetByIdAsync(Guid id);
        PaginatedList<RentHistory> GetPaginatedList(int pageIndex, int pageSize, Guid userId);
    }
}