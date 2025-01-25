using Core.Entities;

namespace Core.Repositiories
{
    public interface IAdvertismentRepository : IBaseRepository<Advertisment>
    {
        Task<List<Advertisment>> Search(
            string? query = null,
            List<Guid>? categories = null,
            int skip = 0,
            int take = 5
            );
    }
}
