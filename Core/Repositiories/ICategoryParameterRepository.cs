using Core.Entities;

namespace Core.Repositiories
{
    public interface ICategoryParameterRepository : IBaseRepository<CategoryParameter>
    {
        Task<List<CategoryParameter>> GetForCategoryAsync(Guid categoryId);
    }
}
