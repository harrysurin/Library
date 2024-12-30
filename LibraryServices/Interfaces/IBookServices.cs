using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IBookServices : IServices<Book>
    {
        Task<Book?> GetBookByISBN(string ISBN);
        Task<List<Book>> GetBooksByAuthor(Guid authorId);
        PaginatedList<Book> PaginatedListByAuthorId(int pageIndex, int pageSize, Guid authorId);
        PaginatedList<Book> PaginatedListByGenre(int pageIndex, int pageSize, string genre);
        PaginatedList<Book> PaginatedListByName(int pageIndex, int pageSize, string book);
        PaginatedList<Book> PaginatedList(int pageIndex, int pageSize);
    }
}