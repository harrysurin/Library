using System.Linq.Expressions;

public interface IServices<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}