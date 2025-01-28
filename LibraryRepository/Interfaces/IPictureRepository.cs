using System.Linq.Expressions;


namespace LibraryRepository.Interfaces;
public interface IPictureRepository<BookPictures> 
{
    Task AddAsync(BookPictures picture, string serverRootPath,  string pathToImagesDirectory, CancellationToken cancellationToken = default);
    Task<BookPictures> GetAsync(Guid pictureId, string serverRootPath, CancellationToken cancellationToken = default);
    void Delete(BookPictures picture, string serverRootPath);
    Task<List<BookPictures>> ToListByPredicateAsync(Expression<Func<BookPictures, bool>> predicate, CancellationToken cancellationToken = default);
}