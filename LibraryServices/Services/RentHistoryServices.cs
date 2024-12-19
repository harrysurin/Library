using System.IO.Compression;
using System.Runtime.CompilerServices;
using LibraryRepository.Models;
using LibraryServices.Interfaces;


public class RentHistoryService : IRentHistoryServices
{
    private readonly IUnitOfWork _unitOfWork;
    public RentHistoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

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

    public async Task<bool> AccessToReturn(Guid bookId)
    {
        var rentHistory = await _unitOfWork.RentHistory
            .FirstOrDefaultAsync(x => x.BookId == bookId && x.DateOfReturn == null);

        if(rentHistory == null) return false;
        else
        return false;
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

}