

using Core.Entities;
using Core.Repositiories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<List<Category>> GetNested(Category parent)
        {
            return await Set.Where(x => x.Parent == parent).ToListAsync();
        }

        public async Task<List<Category>> GetRoots()
        {
            return await Set.Where(x => x.Parent == null).ToListAsync();
        }

        public async Task<int> NestedCount(Category category)
        {
            return await Set.CountAsync(x => x.Parent == category);
        }
    }
}
