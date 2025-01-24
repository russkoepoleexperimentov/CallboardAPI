using Core.Entities;

namespace Core.Repositiories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<List<Category>> GetNested(Category parent);
        Task<List<Category>> GetRoots();
        Task<int> NestedCount(Category category);
    }
}
