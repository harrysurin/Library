using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IAuthorServices : IServices<Author>
    {
        Task<Author?> GetAuthorByName(string AuthorName);
        PaginatedList<Author> GetPaginatedList(int pageIndex, int pageSize);
    }
}

