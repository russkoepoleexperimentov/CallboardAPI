using Core.Entities;
using Core.Repositiories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AdvertisementImageRepository : GenericRepository<AdvertisementImage>, IAdvertisementImageRepository
    {
        public AdvertisementImageRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<List<AdvertisementImage>> GetForAdvertisement(Advertisement advertisement)
        {
            return await Set.Where(x => x.Advertisement == advertisement).ToListAsync();
        }
    }
}
