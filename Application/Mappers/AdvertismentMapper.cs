
using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Mappers
{
    public class AdvertismentMapper : Profile
    {
        public AdvertismentMapper() {
            CreateMap<AdvertismentCreateDto, Advertisment>()
                .ForMember(dest => dest.OldCost, opt => opt.Ignore());

            CreateMap<Advertisment, AdvertismentDto>();
        }
    }
}
