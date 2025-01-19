using FluentValidation;
using LibraryRepository.Models;
using LibraryServices.Interfaces;
using LibraryServices.Validation;


public class AuthorService : IAuthorServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AuthorValidator _validator;
    public AuthorService(IUnitOfWork unitOfWork, AuthorValidator validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
        => await _unitOfWork.Authors.GetAllAsync();
    
    public async Task<Author?> GetByIdAsync(Guid id) => await _unitOfWork.Authors.GetByIdAsync(id);
    public async Task AddAsync(Author author)
    {   
        _validator.ValidateAndThrow(author);
        await _unitOfWork.Authors.AddAsync(author);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Update(Author author)
    {
        _validator.ValidateAndThrow(author);
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
        return await _unitOfWork.Authors.FirstOrDefaultAsync(x 
            => x.FirstName == AuthorName || x.LastName == AuthorName
                || (x.FirstName + " " + x.LastName).Contains(AuthorName));
    }

    public PaginatedList<Author> GetPaginatedList(int pageIndex, int pageSize)
    {
        return _unitOfWork.Authors
            .GetPaginatedListAsync(pageIndex, pageSize, null , x => x.LastName);
    }

}