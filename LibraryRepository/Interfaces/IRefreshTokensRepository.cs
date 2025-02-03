using System.Linq.Expressions;


namespace LibraryRepository.Interfaces;
public interface IRefreshTokensRepository
{
    void Add(RefreshToken refreshToken);
    Task<RefreshToken?> FirstOrDefaultAsync(Expression<Func<RefreshToken, bool>> predicate, CancellationToken token = default);
    void Delete(RefreshToken refreshToken);
}