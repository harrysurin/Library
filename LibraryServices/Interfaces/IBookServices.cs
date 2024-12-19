using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IBookServices : IServices<Book>
    {
        Task<Book?> GetBookByISBN(string ISBN);
        Task<List<Book>> GetBooksByAuthor(Guid authorId);
    }
}