using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IBookPicturesServices
    {
        Task<BookPictures?> GetPictureAsync(Guid pictureId, string serverRootPath);
        Task AddPicture(BookPictures picture, string serverRootPath, string pathToImagesDirectory, CancellationToken cancellationToken = default);
        Task Delete(BookPictures picture, string serverRootPath, CancellationToken cancellationToken = default);
        Task Delete(Guid pictureId, string serverRootPath, CancellationToken cancellationToken);
        Task<List<BookPictures>> GetBookPictures(Guid bookId, CancellationToken cancellationToken = default);
    }
}