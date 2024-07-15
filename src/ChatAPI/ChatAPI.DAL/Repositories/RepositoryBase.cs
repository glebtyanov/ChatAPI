using System.Linq.Expressions;
using ChatAPI.DAL.Data;
using ChatAPI.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.DAL.Repositories
{
    public class RepositoryBase<TEntity>(ChatDbContext context) : IRepositoryBase<TEntity>
        where TEntity : class
    {
        protected readonly ChatDbContext Context = context;

        public async Task CreateAsync(TEntity model)
        {
            Context.Set<TEntity>().Add(model);
            await Context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> models)
        {
            Context.Set<TEntity>().AddRange(models);
            await Context.SaveChangesAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity?> GetWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetListWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() => Context.Set<TEntity>().Where<TEntity>(predicate));
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await Context.Set<TEntity>().CountAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().AnyAsync(predicate);
        }

        public async Task UpdateAsync(TEntity model)
        {
            Context.Entry(model).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public async Task RemoveAsync(TEntity model)
        {
            Context.Set<TEntity>().Remove(model);
            await Context.SaveChangesAsync();
        }
    }
}