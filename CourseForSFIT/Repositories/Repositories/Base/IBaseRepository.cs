using System.Linq.Expressions;

namespace Repositories.Repositories.Base
{
    public interface IBaseRepository<T>
        where T : class
    {
        Task AddAsync(T entity);

        Task RemoveAsync(int id);

        Task UpdateAsync(int id, T entity);

        Task<T> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<int> GetTotalCountAsync();

        IEnumerable<T> GetAll();
        IQueryable<T> GetAllQueryAble();
        Task<TField> GetFieldByIdAsync<TField>(int id, Expression<Func<T, TField>> selector);
        Task AddManyAsync(IEnumerable<T> entities);
    }
}