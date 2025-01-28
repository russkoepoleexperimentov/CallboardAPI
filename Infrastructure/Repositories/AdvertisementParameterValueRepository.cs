using Core.Entities;
using Core.Repositiories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AdvertisementParameterValueRepository : GenericRepository<AdvertisementParameterValue>, IAdvertisementParameterValueRepository
    {
        public AdvertisementParameterValueRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<List<AdvertisementParameterValue>> GetForAdvertisment(Advertisement advertisment)
        {
            return await Set.Where(x => x.Advertisment == advertisment).ToListAsync();
        }
    }
}
