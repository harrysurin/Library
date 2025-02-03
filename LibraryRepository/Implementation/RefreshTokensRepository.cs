using LibraryRepository.Interfaces;
using LibraryRepository.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace LibraryRepository.Implementations;
public class RefreshTokensRepository : IRefreshTokensRepository
{
    private readonly LibraryContext context;
    private readonly DbSet<RefreshToken> dbSet;



    public RefreshTokensRepository(LibraryContext _context)
    {
        context = _context;
        dbSet = context.Set<RefreshToken>();
    }

    public void Add(RefreshToken refreshToken) 
    {
        context.RefreshTokens.Add(refreshToken);
    }

    public async Task<RefreshToken?> FirstOrDefaultAsync(Expression<Func<RefreshToken, bool>> predicate, CancellationToken token = default) 
    {
         return await context.RefreshTokens
            .FirstOrDefaultAsync(predicate, token);
    }

    public void Delete(RefreshToken refreshToken) => context.RefreshTokens.Remove(refreshToken);

    
}