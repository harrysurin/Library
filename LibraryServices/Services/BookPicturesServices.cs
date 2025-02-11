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
        if(picture == null && picture.PictureBytes == null)
        {
            throw new ArgumentNullException();
        }
        return picture;
    }
    
    public async Task AddPicture(BookPictures picture, string serverRootPath, 
                                string pathToImagesDirectory, CancellationToken cancellationToken)
    {
        if (picture.Picture == null)
        {
            throw new ArgumentNullException("Picture isn't uploaded");
        }
        
        if (picture.Picture.Length > 0)
        {
            using (var stream = new MemoryStream())
            {
                picture.Picture.CopyTo(stream);
                picture.PictureBytes = stream.ToArray();
            }
        }

        var skImage = SKImage.FromEncodedData(picture.PictureBytes);
        if (skImage == null)
        {
            throw new ArgumentException("Provided file isn't an image");
        }

        using var codec = SKCodec.Create(skImage.EncodedData);
        var format = codec.EncodedFormat.ToString().ToLower();
        picture.FileExtension = format;

        await _unitOfWork.BookPictures.AddAsync(picture, serverRootPath, pathToImagesDirectory);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task Delete(BookPictures picture, string serverRootPath, CancellationToken cancellationToken)
    {
        if(picture != null)
        {
            throw new ArgumentNullException();
        }
        _unitOfWork.BookPictures.Delete(picture, serverRootPath);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<List<BookPictures>> GetBookPictures(Guid bookId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.BookPictures.ToListByPredicateAsync(x => x.BookId == bookId, cancellationToken);
    }
}