using Data.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task UpdateAsync(int id, T entity)
        {
            var res = await _dbSet.FindAsync(id);
            _context.Entry(entity).State = EntityState.Modified;
            if (res != null)
            {
                _dbSet.Update(res);
            }
        }
    }
}