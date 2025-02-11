using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IBookServices : IServices<Book>
    {
        Task AddAsync(Book book, BookPictures picture, string serverRootPath,
                                        string pathToImagesDirectory, CancellationToken cancellationToken);
        Task<Book?> GetBookByISBN(string ISBN, CancellationToken cancellationToken = default);
        Task<List<Book>> GetBooksByAuthor(Guid authorId, CancellationToken cancellationToken = default);
        PaginatedList<Book> GetPaginatedListByAuthorId(int pageIndex, int pageSize, Guid authorId);
        PaginatedList<Book> GetPaginatedListByGenre(int pageIndex, int pageSize, string genre);
        PaginatedList<Book> GetPaginatedListByName(int pageIndex, int pageSize, string book);
        PaginatedList<Book> GetPaginatedList(int pageIndex, int pageSize);
    }
}