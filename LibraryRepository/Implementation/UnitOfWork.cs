using LibraryRepository.Interfaces;
using LibraryRepository.Models;

namespace LibraryRepository.Implementations;
public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryContext _context;
    private IRepository<Author> _authors;
    private IRepository<Book> _books;
    private IRepository<RentHistory> _rentHistory;
    private IPictureRepository<BookPictures> _picture;
    private IRefreshTokensRepository _refreshToken;
    public UnitOfWork(LibraryContext context)
    {
        _context = context;
    }
    public IRepository<Author> Authors => _authors ??= new Repository<Author>(_context);
    public IRepository<Book> Books => _books ??= new Repository<Book>(_context);
    public IRepository<RentHistory> RentHistory => _rentHistory ??= new Repository<RentHistory>(_context);
    public IPictureRepository<BookPictures> BookPictures => _picture ??= new PictureRepository(_context);
    public IRefreshTokensRepository RefreshTokens => _refreshToken ??= new RefreshTokensRepository(_context);
    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default) 
        => await _context.SaveChangesAsync(cancellationToken);
    public void Dispose() => _context. Dispose();
}