using Common.Enums;
using Core.Entities;

namespace Core.Repositiories
{
    public interface IAdvertisementRepository : IBaseRepository<Advertisement>
    {
        Task<List<Advertisement>> Search(
            string? query = null,
            List<Guid>? categories = null,
            AdvertisementSorting sorting = AdvertisementSorting.DateAsc,
            int skip = 0,
            int take = 5
            );
    }
}
