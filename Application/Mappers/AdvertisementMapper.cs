
using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Mappers
{
    public class AdvertisementMapper : Profile
    {
        public AdvertisementMapper() {
            CreateMap<AdvertisementCreateDto, Advertisement>()
                .ForMember(dest => dest.OldCost, opt => opt.Ignore());

            CreateMap<Advertisement, AdvertisementDto>();
        }
    }
}
