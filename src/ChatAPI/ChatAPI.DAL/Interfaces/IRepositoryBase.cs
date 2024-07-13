using System.Linq.Expressions;

namespace ChatAPI.DAL.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetWhereAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetListWhereAsync(Expression<Func<T, bool>> predicate);
        Task RemoveAsync(T model);
        Task AddAsync(T model);
        Task AddRangeAsync(IEnumerable<T> models);
        Task UpdateAsync(T model);
        Task<int> CountAsync();
    }
}