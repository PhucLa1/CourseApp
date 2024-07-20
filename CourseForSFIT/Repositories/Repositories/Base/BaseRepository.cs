using Data.Data;
using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
namespace Repositories.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        protected readonly CourseForSFITContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(CourseForSFITContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public IEnumerable<T> GetAll()
        {
            var res = _dbSet.ToList();
            return res;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var res = await _dbSet.ToListAsync();
            return res;
        }

        public async Task<IEnumerable<T>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            var res = await _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return res;
        }

        public async Task<int> GetTotalCountAsync()
        {
            var res = await _dbSet.CountAsync();
            return res;
        }
        public async Task<T> GetByIdAsync(int id)
        {
            var res = await _dbSet.FindAsync(id);
            return res;
        }

        public async Task RemoveAsync(int id)
        {
            var res = await _dbSet.FindAsync(id);
            if (res != null)
            {
                _dbSet.Remove(res);
            }
        }

        public DbSet<T> GetDbSet()
        {
            return _dbSet;
        }

        public async Task<TField> GetFieldByIdAsync<TField>(int id, Expression<Func<T, TField>> selector)
        {
            var res = await _dbSet.Where($"Id == @0", id).Select(selector).FirstOrDefaultAsync();
            return res;
        }

        public IQueryable<T> GetAllQueryAble()
        {
            return _dbSet;
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var res = await _dbSet.FindAsync(id);
            _context.Entry(entity).State = EntityState.Modified;
            if (res != null)
            {
                _dbSet.Update(res);
            }
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task AddManyAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }
        public void UpdateMany(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }
    }
}