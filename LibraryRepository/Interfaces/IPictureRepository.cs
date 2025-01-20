using System.Linq.Expressions;


namespace LibraryRepository.Interfaces;
public interface IPictureRepository<BookPictures> 
{
    Task AddAsync(BookPictures picture, string serverRootPath,  string pathToImagesDirectory);
    Task<BookPictures> GetAsync(Guid pictureId);
    void Delete(BookPictures picture, string serverRootPath);
    Task<List<BookPictures>> ToListByPredicateAsync(Expression<Func<BookPictures, bool>> predicate);
}