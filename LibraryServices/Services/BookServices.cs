using LibraryRepository.Models;
using LibraryServices.Interfaces;
using LibraryServices.Validation;
using FluentValidation;
using LibraryRepository.Interfaces;


public class BookService : IBookServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookPicturesServices bookPicturesService;
    private readonly BookValidator _validator;
    public BookService(IUnitOfWork unitOfWork,
        BookValidator validator,
        IBookPicturesServices _bookPicturesService)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        bookPicturesService = _bookPicturesService;
    }

    public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken)
        => await _unitOfWork.Books.GetAllAsync(cancellationToken);
    
    public async Task AddAsync(Book book, CancellationToken cancellationToken)
    { 
        _validator.ValidateAndThrow(book);
        await _unitOfWork.Books.AddAsync(book, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task AddAsync(Book book, BookPictures picture, string serverRootPath,
                                        string pathToImagesDirectory, CancellationToken cancellationToken)
    {
        await this.AddAsync(book, cancellationToken);
        if (picture != null)
        {
            await bookPicturesService.AddPicture(picture, serverRootPath, pathToImagesDirectory, cancellationToken);
        }
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
       var book = await _unitOfWork.Books.GetByIdAsync(id);
        if(book == null)
        {
            throw new ArgumentNullException();
        }
        else
        {
            return book;
        }
    }

    public async Task Update(Book book, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(book);
        _unitOfWork.Books.Update(book);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task Delete(Book book, CancellationToken cancellationToken)
    {
        if(book == null)
        {
            throw new ArgumentNullException();
        }
        else
        {
            _unitOfWork.Books.Delete(book);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }

    public async Task<Book?> GetBookByISBN(string ISBN, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.FirstOrDefaultAsync(x => x.ISBN == ISBN, cancellationToken);
        if(book == null)
        {
            throw new ArgumentNullException();
        }
        else
        return book;
    }

    public async Task<List<Book>> GetBooksByAuthor(Guid authorId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Books.ToListByPredicateAsync(x => x.AuthorId == authorId, cancellationToken);
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