using System.Linq.Expressions;


namespace LibraryRepository.Interfaces;
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<T>> ToListByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    PaginatedList<T> GetPaginatedList<TKey>(int pageIndex, int pageSize,
                 Expression<Func<T, bool>>? filterPredicate, Func<T, TKey> orderPredicate);
}