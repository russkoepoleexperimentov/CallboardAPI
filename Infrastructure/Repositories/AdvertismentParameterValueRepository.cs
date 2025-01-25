using Core.Entities;
using Core.Repositiories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AdvertismentParameterValueRepository : GenericRepository<AdvertismentParameterValue>, IAdvertismentParameterValueRepository
    {
        public AdvertismentParameterValueRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<List<AdvertismentParameterValue>> GetForAdvertisment(Advertisment advertisment)
        {
            return await Set.Where(x => x.Advertisment == advertisment).ToListAsync();
        }
    }
}
