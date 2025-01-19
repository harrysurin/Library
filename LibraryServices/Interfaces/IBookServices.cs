using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IBookServices : IServices<Book>
    {
        Task<Book?> GetBookByISBN(string ISBN);
        Task<List<Book>> GetBooksByAuthor(Guid authorId);
        PaginatedList<Book> GetPaginatedListByAuthorId(int pageIndex, int pageSize, Guid authorId);
        PaginatedList<Book> GetPaginatedListByGenre(int pageIndex, int pageSize, string genre);
        PaginatedList<Book> GetPaginatedListByName(int pageIndex, int pageSize, string book);
        PaginatedList<Book> GetPaginatedList(int pageIndex, int pageSize);
    }
}