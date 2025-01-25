

using Core.Entities;

namespace Core.Repositiories
{
    public interface IAdvertismentParameterValueRepository : IBaseRepository<AdvertismentParameterValue>
    {
        Task<List<AdvertismentParameterValue>> GetForAdvertisment(Advertisment advertisment);
    }
}
