using LibraryRepository.Models;
using LibraryServices.Interfaces;


public class BookService : IBookServices
{
    private readonly IUnitOfWork _unitOfWork;
    public BookService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
        => await _unitOfWork.Books.GetAllAsync();
    
    public async Task AddAsync(Book book)
    { 
        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<Book?> GetByIdAsync(Guid id) => await _unitOfWork.Books.GetByIdAsync(id);

    public async Task Update(Book book)
    {
        _unitOfWork.Books.Update(book);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(Book book)
    {
        _unitOfWork.Books.Delete(book);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<Book?> GetBookByISBN(string ISBN)
    {
        return await _unitOfWork.Books.FirstOrDefaultAsync(x => x.ISBN == ISBN);
    }

    public async Task<List<Book>> GetBooksByAuthor(Guid authorId)
    {
        return await _unitOfWork.Books.ToListByPredicateAsync(x => x.AuthorId == authorId);
    }

    public PaginatedList<Book> PaginatedListByAuthorId(int pageIndex, int pageSize, Guid authorId)
    {
        return _unitOfWork.Books
            .GetAllPaginatedAsync(pageIndex, pageSize, x => x.AuthorId == authorId, x => x.Title);
    }

    public PaginatedList<Book> PaginatedListByGenre(int pageIndex, int pageSize, string genre)
    {
        return _unitOfWork.Books
            .GetAllPaginatedAsync(pageIndex, pageSize, x => x.Genre == genre, x => x.Title);
    }

    public PaginatedList<Book> PaginatedListByName(int pageIndex, int pageSize, string book)
    {
        return _unitOfWork.Books
            .GetAllPaginatedAsync(pageIndex, pageSize, x => x.Title == book, x => x.Title);
    }

    public PaginatedList<Book> PaginatedList(int pageIndex, int pageSize)
    {
        return _unitOfWork.Books
            .GetAllPaginatedAsync(pageIndex, pageSize, null , x => x.Title);
    }
}