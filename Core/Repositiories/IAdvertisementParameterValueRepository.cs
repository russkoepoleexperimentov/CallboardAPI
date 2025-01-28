

using Core.Entities;

namespace Core.Repositiories
{
    public interface IAdvertisementParameterValueRepository : IBaseRepository<AdvertisementParameterValue>
    {
        Task<List<AdvertisementParameterValue>> GetForAdvertisment(Advertisement advertisment);
    }
}
