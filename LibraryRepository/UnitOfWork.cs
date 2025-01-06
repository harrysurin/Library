using LibraryRepository.Models;

public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryContext _context;
    private IRepository<Author> _authors;
    private IRepository<Book> _books;
    private IRepository<RentHistory> _rentHistory;
    private IPictureRepository<BookPictures> _picture;
    public UnitOfWork(LibraryContext context)
    {
        _context = context;
    }
    public IRepository<Author> Authors => _authors ??= new Repository<Author>(_context);
    public IRepository<Book> Books => _books ??= new Repository<Book>(_context);
    public IRepository<RentHistory> RentHistory => _rentHistory ??= new Repository<RentHistory>(_context);
    public IPictureRepository<BookPictures> BookPictures => _picture ??= new PictureRepository(_context);
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context. Dispose();
}