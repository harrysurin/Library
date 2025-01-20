using LibraryRepository.Models;
using LibraryServices.Interfaces;
using LibraryServices.Validation;
using FluentValidation;
using LibraryRepository.Interfaces;


public class BookService : IBookServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly BookValidator _validator;
    public BookService(IUnitOfWork unitOfWork, BookValidator validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
        => await _unitOfWork.Books.GetAllAsync();
    
    public async Task AddAsync(Book book)
    { 
        _validator.ValidateAndThrow(book);
        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<Book?> GetByIdAsync(Guid id) => await _unitOfWork.Books.GetByIdAsync(id);

    public async Task Update(Book book)
    {
        _validator.ValidateAndThrow(book);
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

    public PaginatedList<Book> GetPaginatedListByAuthorId(int pageIndex, int pageSize, Guid authorId)
    {
        return _unitOfWork.Books
            .GetPaginatedList(pageIndex, pageSize, x => x.AuthorId == authorId, x => x.Title);
    }

    public PaginatedList<Book> GetPaginatedListByGenre(int pageIndex, int pageSize, string genre)
    {
        return _unitOfWork.Books
            .GetPaginatedList(pageIndex, pageSize, 
            x =>  String.Equals(x.Genre, genre, StringComparison.OrdinalIgnoreCase) , x => x.Title);
    }

    public PaginatedList<Book> GetPaginatedListByName(int pageIndex, int pageSize, string book)
    {
        return _unitOfWork.Books
            .GetPaginatedList(pageIndex, pageSize, x => x.Title.ToUpper().Contains(book.ToUpper()),
                x => x.Title);
    }

    public PaginatedList<Book> GetPaginatedList(int pageIndex, int pageSize)
    {
        return _unitOfWork.Books
            .GetPaginatedList(pageIndex, pageSize, null , x => x.Title);
    }
}