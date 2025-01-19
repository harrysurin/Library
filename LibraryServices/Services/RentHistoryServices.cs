using System.IO.Compression;
using System.Runtime.CompilerServices;
using LibraryRepository.Models;
using LibraryServices.Interfaces;
using LibraryServices.Validation;
using FluentValidation;


public class RentHistoryService : IRentHistoryServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RentHistoryValidator _validator;

    public RentHistoryService(IUnitOfWork unitOfWork, RentHistoryValidator validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<IEnumerable<RentHistory>> GetAllAsync()
    => await _unitOfWork.RentHistory.GetAllAsync();

    public async Task BookDistribution(Guid userId, Guid bookId)
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
        await _unitOfWork.RentHistory.AddAsync(rentHistory);
        await _unitOfWork.CompleteAsync();
    }

    public async Task ReturnBook(Guid bookId, Guid userId)
    {
        var rentHistory = await _unitOfWork.RentHistory
            .FirstOrDefaultAsync(x => x.BookId == bookId && x.DateOfReturn == null && x.UserId == userId);

        if(rentHistory == null)
        {
            throw new ArgumentException("You don't have access");
        }
        else 
        {
            rentHistory.DateOfReturn = DateTime.Now;
            await _unitOfWork.CompleteAsync();
        }
        
    }

    public async Task<bool> AccessToRent(Guid bookId)
    {
        var rentHistory = await _unitOfWork.RentHistory
            .FirstOrDefaultAsync(x => x.BookId == bookId && x.DateOfReturn == null);

        if(rentHistory == null) return false;
        else
        return true;
    }

    public async Task<List<RentHistory>> UserRentHistory(Guid userId)
    {
        return await _unitOfWork.RentHistory.ToListByPredicateAsync(x => x.UserId == userId);
    }

    public async Task Delete(RentHistory rentHistory) 
    {
        _unitOfWork.RentHistory.Delete(rentHistory);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<RentHistory?> GetByIdAsync(Guid id) => await _unitOfWork.RentHistory.GetByIdAsync(id);

    public PaginatedList<RentHistory> PaginatedList(int pageIndex, int pageSize, Guid userId)
    {
        return _unitOfWork.RentHistory
            .GetAllPaginatedAsync(pageIndex, pageSize, x => x.UserId == userId, x => x.DateOfRent);
    }

}