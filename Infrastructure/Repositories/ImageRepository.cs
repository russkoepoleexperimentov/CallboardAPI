
using Core.Entities;
using Core.Repositiories;

namespace Infrastructure.Repositories
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        public ImageRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
