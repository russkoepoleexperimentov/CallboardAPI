using Core.Entities;
using Core.Repositiories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryParameterRepository : GenericRepository<CategoryParameter>, ICategoryParameterRepository
    {
        public CategoryParameterRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<List<CategoryParameter>> GetForCategoryAsync(Guid categoryId)
        {
            return await Set.Where(x => x.Category.Id == categoryId).ToListAsync();
        }
    }
}
