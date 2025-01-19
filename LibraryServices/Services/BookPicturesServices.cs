using LibraryRepository.Models;
using LibraryServices.Interfaces;

public class BookPicturesServices : IBookPicturesServices
{
    private readonly IUnitOfWork _unitOfWork;
    public BookPicturesServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BookPictures?> GetPictureAsync(Guid pictureId) 
        => await _unitOfWork.BookPictures.GetAsync(pictureId);

    
    public async Task AddPicture(BookPictures picture, string serverRootPath, string pathToImagesDirectory)
    {
        await _unitOfWork.BookPictures.AddAsync(picture, serverRootPath, pathToImagesDirectory);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(BookPictures picture, string serverRootPath)
    {
        _unitOfWork.BookPictures.Delete(picture, serverRootPath);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<List<BookPictures>> GetBookPictures(Guid bookId)
    {
        return await _unitOfWork.BookPictures.ToListByPredicateAsync(x => x.BookId == bookId);
    }
}