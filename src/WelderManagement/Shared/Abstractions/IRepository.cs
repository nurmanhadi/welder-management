namespace WelderManagement.Shared.Abstractions;

public interface IRepository<T>
{
    Task<T?> GetByIdAsync(Guid id);
    Task<int> CountByIdAsync(Guid id);
    Task InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
