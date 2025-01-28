using LibraryRepository.Models;
using LibraryServices.Interfaces;
using LibraryServices.Validation;
using FluentValidation;
using LibraryRepository.Interfaces;


public class RentHistoryService : IRentHistoryServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RentHistoryValidator _validator;

    public RentHistoryService(IUnitOfWork unitOfWork, RentHistoryValidator validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<IEnumerable<RentHistory>> GetAllAsync(CancellationToken cancellationToken)
    => await _unitOfWork.RentHistory.GetAllAsync(cancellationToken);

    public async Task BookRent(Guid userId, Guid bookId, CancellationToken cancellationToken)
    {
        RentHistory rentHistory = new RentHistory()
        {
            Id = new Guid(),
            UserId = userId,
            BookId = bookId,
            DateOfRent = DateTime.Now,
            DateOfReturn = null
        };
        _validator.ValidateAndThrow(rentHistory);
        await _unitOfWork.RentHistory.AddAsync(rentHistory, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task ReturnBook(Guid bookId, Guid userId, CancellationToken cancellationToken)
    {
        var rentHistory = await _unitOfWork.RentHistory
            .FirstOrDefaultAsync(x => x.BookId == bookId && x.DateOfReturn == null && x.UserId == userId, cancellationToken);

        if(rentHistory == null)
        {
            throw new ArgumentException("No rent records was found");
        }
        else 
        {
            rentHistory.DateOfReturn = DateTime.Now;
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
        
    }

    public async Task<bool> IsAvailableToRent(Guid bookId, CancellationToken cancellationToken)
    {
        var rentHistory = await _unitOfWork.RentHistory
            .FirstOrDefaultAsync(x => x.BookId == bookId && x.DateOfReturn == null, cancellationToken);

        return rentHistory != null;
    }

    public async Task<List<RentHistory>> GetUserRentHistory(Guid userId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.RentHistory.ToListByPredicateAsync(x => x.UserId == userId, cancellationToken);
    }

    public async Task Delete(RentHistory rentHistory, CancellationToken cancellationToken) 
    {
        _unitOfWork.RentHistory.Delete(rentHistory);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<RentHistory?> GetByIdAsync(Guid id) => await _unitOfWork.RentHistory.GetByIdAsync(id);

    public PaginatedList<RentHistory> GetPaginatedList(int pageIndex, int pageSize, Guid userId)
    {
        return _unitOfWork.RentHistory
            .GetPaginatedList(pageIndex, pageSize, x => x.UserId == userId, x => x.DateOfRent);
    }

}