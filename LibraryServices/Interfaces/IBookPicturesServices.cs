using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IBookPicturesServices
    {
        Task<BookPictures?> GetPictureAsync(Guid pictureId);
        Task AddPicture(BookPictures picture, string serverRootPath, string pathToImagesDirectory);
        Task Delete(BookPictures picture, string serverRootPath);
        Task<List<BookPictures>> GetPicturesByBook(Guid bookId);
    }
}