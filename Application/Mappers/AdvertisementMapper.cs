
using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Mappers
{
    public class AdvertisementMapper : Profile
    {
        public AdvertisementMapper() 
        {
            CreateMap<AdvertisementCreateDto, Advertisement>();

            CreateMap<Advertisement, AdvertisementDto>();

            CreateMap<AdvertisementUpdateDto, Advertisement>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
