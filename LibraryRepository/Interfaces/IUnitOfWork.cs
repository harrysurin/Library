using LibraryRepository.Models;


namespace LibraryRepository.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IRepository<Author> Authors { get; }
    IRepository<Book> Books { get; }
    IRepository<RentHistory> RentHistory { get; }
    IPictureRepository<BookPictures> BookPictures { get; }
    IRefreshTokensRepository RefreshTokens { get; }
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}