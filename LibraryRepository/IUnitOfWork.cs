using LibraryRepository.Models;

public interface IUnitOfWork : IDisposable
{
    IRepository<Author> Authors { get; }
    IRepository<Book> Books { get; }
    IRepository<RentHistory> RentHistory { get; }
    Task<int> CompleteAsync();
}