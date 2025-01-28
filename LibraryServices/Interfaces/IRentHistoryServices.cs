using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IRentHistoryServices
    {
        Task<IEnumerable<RentHistory>> GetAllAsync(CancellationToken cancellationToken = default);
        Task BookRent(Guid userId, Guid bookId, CancellationToken cancellationToken = default);
        Task ReturnBook(Guid bookId, Guid userId, CancellationToken cancellationToken = default);
        Task<bool> IsAvailableToRent(Guid bookId, CancellationToken cancellationToken = default);
        Task<List<RentHistory>> GetUserRentHistory(Guid userId, CancellationToken cancellationToken = default);
        Task Delete(RentHistory rentHistory, CancellationToken cancellationToken = default);
        Task<RentHistory?> GetByIdAsync(Guid id);
        PaginatedList<RentHistory> GetPaginatedList(int pageIndex, int pageSize, Guid userId);
    }
}