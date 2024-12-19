using LibraryRepository.Models;
using LibraryServices.Interfaces;


public class AuthorService : IAuthorServices
{
    private readonly IUnitOfWork _unitOfWork;
    public AuthorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
        => await _unitOfWork.Authors.GetAllAsync();
    
    public async Task<Author?> GetByIdAsync(Guid id) => await _unitOfWork.Authors.GetByIdAsync(id);
    public async Task AddAsync(Author author)
    { 
        await _unitOfWork.Authors.AddAsync(author);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Update(Author author)
    {
        _unitOfWork.Authors.Update(author);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(Author author)
    {
        _unitOfWork.Authors.Delete(author);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<Author?> GetAuthorByName(string AuthorName)
    {
        return await _unitOfWork.Authors.FirstOrDefaultAsync(x => x.Name == AuthorName);
    }



}