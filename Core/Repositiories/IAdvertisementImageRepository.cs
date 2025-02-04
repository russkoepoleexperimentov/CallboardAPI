
using Core.Entities;

namespace Core.Repositiories
{
    public interface IAdvertisementImageRepository : IBaseRepository<AdvertisementImage>
    {
        Task<List<AdvertisementImage>> GetForAdvertisement(Advertisement advertisement);
    }
}
