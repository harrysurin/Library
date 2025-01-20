using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IAuthorServices : IServices<Author>
    {
        Task<IEnumerable<Author>> GetAuthorByName(string AuthorName);
        PaginatedList<Author> GetPaginatedList(int pageIndex, int pageSize);
    }
}

