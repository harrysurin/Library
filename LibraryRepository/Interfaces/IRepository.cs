using System.Linq.Expressions;


namespace LibraryRepository.Interfaces;
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> ToListByPredicateAsync(Expression<Func<T, bool>> predicate);

    PaginatedList<T> GetPaginatedListAsync<TKey>(int pageIndex, int pageSize,
                 Expression<Func<T, bool>>? filterPredicate, Func<T, TKey> orderPredicate);
}