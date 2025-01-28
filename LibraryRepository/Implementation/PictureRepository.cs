using LibraryRepository.Interfaces;
using LibraryRepository.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace LibraryRepository.Implementations;
public class PictureRepository : IPictureRepository<BookPictures>
{
    private readonly LibraryContext _context;
    private readonly DbSet<BookPictures> _dbSet;
    public PictureRepository(LibraryContext context)
    {
        _context = context;
        _dbSet = _context.Set<BookPictures>();
    }

    public async Task AddAsync(BookPictures picture, string serverRootPath, 
                            string pathToImagesDirectory, CancellationToken cancellationToken)
    {
        if (picture.PictureBytes == null)
        {
            throw new ArgumentNullException("Picture is null");
        }

        picture.Path = Path.Combine(pathToImagesDirectory, picture.Id + "." + picture.FileExtension);
        await _dbSet.AddAsync(picture, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var fullPath = Path.Combine(serverRootPath, picture.Path);
        File.WriteAllBytes(fullPath, picture.PictureBytes);
    }

    public async Task<BookPictures> GetAsync(Guid pictureId, string serverRootPath, CancellationToken cancellationToken)
    {
        var picture = await _dbSet.FirstOrDefaultAsync(x => x.Id == pictureId, cancellationToken);
        
        if (picture == null)
        {
            throw new ArgumentException("Picture not found");
        }

        var fullPath = Path.Combine(serverRootPath, picture.Path);
        picture.PictureBytes = File.ReadAllBytes(fullPath);

        return picture;
    }

    public void Delete(BookPictures picture, string serverRootPath)
    {
        var fullPath = Path.Combine(serverRootPath, picture.Path);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        _dbSet.Remove(picture);
    }

    public async Task<List<BookPictures>> ToListByPredicateAsync(Expression<Func<BookPictures, bool>> predicate, CancellationToken cancellationToken) 
        => await _dbSet.Where(predicate).ToListAsync(cancellationToken);

}