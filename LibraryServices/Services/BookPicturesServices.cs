using LibraryRepository.Interfaces;
using LibraryRepository.Models;
using LibraryServices.Interfaces;
using SkiaSharp;

public class BookPicturesServices : IBookPicturesServices
{
    private readonly IUnitOfWork _unitOfWork;
    public BookPicturesServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BookPictures?> GetPictureAsync(Guid pictureId, string serverRootPath) 
    {
        var picture = await _unitOfWork.BookPictures.GetAsync(pictureId, serverRootPath);
        
        var skImage = SKImage.FromEncodedData(picture.PictureBytes);
        using var codec = SKCodec.Create(skImage.EncodedData);
        var format = codec.EncodedFormat.ToString().ToLower();
        picture.FileExtension = format;

        return picture;
    }
    
    public async Task AddPicture(BookPictures picture, string serverRootPath, string pathToImagesDirectory)
    {
        var skImage = SKImage.FromEncodedData(picture.PictureBytes);
        if (skImage == null)
        {
            throw new ArgumentException("Provided file isn't an image");
        }

        using var codec = SKCodec.Create(skImage.EncodedData);
        var format = codec.EncodedFormat.ToString().ToLower();
        picture.FileExtension = format;

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