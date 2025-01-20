using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LibraryRepository.Interfaces;


namespace LibraryRepository.Implementations;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly LibraryContext _context;
    private readonly DbSet<T> _dbSet;
    public Repository(LibraryContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);
    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public void Update(T entity) => _dbSet.Update(entity);
    public void Delete(T entity) => _dbSet.Remove(entity);
    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate) 
        => await _dbSet.FirstOrDefaultAsync(predicate);
    public async Task<List<T>> ToListByPredicateAsync(Expression<Func<T, bool>> predicate) 
        => await _dbSet.Where(predicate).ToListAsync();

    public PaginatedList<T> GetPaginatedList<TKey>(int pageIndex, int pageSize,
                 Expression<Func<T, bool>>? filterPredicate, Func<T, TKey> orderKeySelector)
    {
        if(pageIndex < 1 || pageSize < 1)
        {
            throw new ArgumentException("Page index/size must be > 0");
        }

        var items = _dbSet
            .AsQueryable();
        if (filterPredicate != null)
        {
            items = items.Where(filterPredicate);
        }
        var itemsAsList = items
            .OrderBy(orderKeySelector)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        int count = itemsAsList.Count;
        int totalPages = (int)Math.Ceiling(count / (double)pageSize);

        return new PaginatedList<T>(itemsAsList, pageIndex, totalPages);
    }
    
}