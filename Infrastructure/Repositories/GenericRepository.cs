

using Core.Entities;
using Core.Repositiories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext _context;
        protected DbSet<T> Set => _context.Set<T>();

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await Set.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Set.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            await Set.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            Set.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await Set.FindAsync(id);
            if (entity != null)
            {
                Set.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
