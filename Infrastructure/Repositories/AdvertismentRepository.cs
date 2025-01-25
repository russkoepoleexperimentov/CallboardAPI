
using Core.Entities;
using Core.Repositiories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AdvertismentRepository : GenericRepository<Advertisment>, IAdvertismentRepository
    {
        public AdvertismentRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<List<Advertisment>> Search(
            string? query = null, 
            List<Guid>? categories = null, 
            int skip = 0, 
            int take = 5
            )
        {
            query ??= "";
            query = query.ToUpper();

            var response = Set.Where(x => x.Title.ToUpper().Contains(query));

            if(categories != null && categories.Count > 0)
                response = response.Where(x => categories.Contains(x.Category.Id));

            return await response.Skip(skip).Take(take).ToListAsync();
        }
    }
}
