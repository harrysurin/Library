using LibraryRepository.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class PictureRepository : IPictureRepository<BookPictures>
{
    private readonly LibraryContext _context;
    private readonly DbSet<BookPictures> _dbSet;
    public PictureRepository(LibraryContext context)
    {
        _context = context;
        _dbSet = _context.Set<BookPictures>();
    }

    public async Task AddAsync(BookPictures picture, string serverRootPath, string pathToImagesDirectory)
    {
        picture.Path = Path.Combine(pathToImagesDirectory, picture.Id + ".jpg");
        await _dbSet.AddAsync(picture);
        await _context.SaveChangesAsync();

        var path = Path.Combine(serverRootPath, picture.Path);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            await picture.Picture.CopyToAsync(stream);
            stream.Close();
        }
    }

    public async Task<BookPictures> GetAsync(Guid pictureId)
    {
        var picture = await _dbSet.FirstOrDefaultAsync(x => x.Id == pictureId);
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

    public async Task<List<BookPictures>> ToListByPredicateAsync(Expression<Func<BookPictures, bool>> predicate) 
        => await _dbSet.Where(predicate).ToListAsync();

}