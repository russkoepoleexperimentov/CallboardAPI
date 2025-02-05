using Common.Enums;
using Core.Entities;
using System.Text.Json;

namespace Core.Repositiories
{
    public interface IAdvertisementRepository : IBaseRepository<Advertisement>
    {
        Task<List<Advertisement>> Search(
            string? query = null,
            List<Guid>? categories = null,
            AdvertisementSorting sorting = AdvertisementSorting.DateAsc,
            Dictionary<Guid, List<JsonElement>> parameterEqualsCriteria = null!,
            Dictionary<Guid, (JsonElement Min, JsonElement Max)> parameterRangeCriteria = null!,
            int costMin = 0,
            int costMax = int.MaxValue,
            bool onlyWithImages = false,
            int skip = 0,
            int take = 5);
    }
}
