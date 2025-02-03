using LibraryRepository.Models;
using LibraryServices.Interfaces;
using LibraryRepository.Interfaces;

public class RefreshTokensServices : IRefreshTokensService
{
    private readonly IUnitOfWork unitOfWork;

    public RefreshTokensServices(IUnitOfWork _unitOfWork)
    {
        unitOfWork = _unitOfWork;
    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        unitOfWork.RefreshTokens.Add(refreshToken);
        await unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task Delete(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        unitOfWork.RefreshTokens.Delete(refreshToken);
        await unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<RefreshToken?> GetTokenAsync(string tokenId, Guid userId, CancellationToken cancellationToken)
    {
        return await unitOfWork.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == tokenId && rt.UserId == userId, cancellationToken);
    }
}