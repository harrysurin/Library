using FluentValidation;
using LibraryRepository.Interfaces;
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

    public async Task<IEnumerable<Author>> GetAllAsync(CancellationToken cancellationToken)
        => await _unitOfWork.Authors.GetAllAsync(cancellationToken);
    
    public async Task<Author?> GetByIdAsync(Guid id)
    {
        var author = await _unitOfWork.Authors.GetByIdAsync(id);
        if(author == null)
        {
            throw new ArgumentNullException();
        }
        else
        {
            return author;
        }
    }    
    public async Task AddAsync(Author author, CancellationToken cancellationToken)
    {   
        _validator.ValidateAndThrow(author);
        await _unitOfWork.Authors.AddAsync(author, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task Update(Author author, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(author);
        _unitOfWork.Authors.Update(author);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task Delete(Author author, CancellationToken cancellationToken)
    {
        if(author == null)
        {
            throw new ArgumentNullException();
        }
        else
        {
            _unitOfWork.Authors.Delete(author);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<Author>> GetAuthorByName(string authorName, CancellationToken cancellationToken)
    {
        authorName = authorName.Trim();
        return await _unitOfWork.Authors.ToListByPredicateAsync(x 
            => String.Equals(x.FirstName, authorName, StringComparison.OrdinalIgnoreCase) 
                || String.Equals(x.LastName, authorName, StringComparison.OrdinalIgnoreCase)
                || String.Equals(x.FirstName + " " + x.LastName, authorName, StringComparison.OrdinalIgnoreCase), cancellationToken);
    }

    public PaginatedList<Author> GetPaginatedList(int pageIndex, int pageSize)
    {
        return _unitOfWork.Authors
            .GetPaginatedList(pageIndex, pageSize, null , x => x.LastName);
    }

}