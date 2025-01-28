
using Common.Enums;
using Core.Entities;
using Core.Repositiories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AdvertisementRepository : GenericRepository<Advertisement>, IAdvertisementRepository
    {
        public AdvertisementRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<List<Advertisement>> Search(
            string? query = null, 
            List<Guid>? categories = null,
            AdvertisementSorting sorting = AdvertisementSorting.DateAsc,
            int skip = 0, 
            int take = 5
            )
        {
            query ??= "";
            query = query.ToUpper();

            var response = Set.Where(x => x.Title.ToUpper().Contains(query));

            if(categories != null && categories.Count > 0)
                response = response.Where(x => categories.Contains(x.Category.Id));

            switch (sorting)
            {
                case AdvertisementSorting.DateAsc:
                    response = response.OrderBy(x => x.CreatedAt);
                    break;
                case AdvertisementSorting.DateDesc:
                    response = response.OrderByDescending(x => x.CreatedAt);
                    break;
                case AdvertisementSorting.CostAsc:
                    response = response.OrderBy(x => x.Cost);
                    break;
                case AdvertisementSorting.CostDesc:
                    response = response.OrderByDescending(x => x.Cost);
                    break;
                case AdvertisementSorting.TitleAsc:
                    response = response.OrderBy(x => x.Title);
                    break;
                case AdvertisementSorting.TitleDesc:
                    response = response.OrderByDescending(x => x.Title);
                    break;
            }

            return await response.Skip(skip).Take(take).ToListAsync();
        }
    }
}
