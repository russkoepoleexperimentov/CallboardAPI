using Application.DTOs;
using AutoMapper;
using Core.Entities;
namespace Application.Mappers
{
    public class CategoryParameterMapper : Profile
    {
        public CategoryParameterMapper()
        {
            CreateMap<CategoryParameter, CategoryParameterDto>()
                .ForMember(dest => dest.CategoryId, 
                opt => opt.MapFrom((x, y) => x.Category.Id))
                .ForMember(dest => dest.EnumValues,
                opt => opt.MapFrom((x, y) => x.DataType == Common.Enums.ParameterDataType.Enum ? x.EnumValues!.Split(',') : null));

            CreateMap<CategoryParameterCreateDto, CategoryParameter>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.EnumValues, opt => opt.MapFrom((x, y) => x.EnumValues != null ? string.Join(',', x.EnumValues!) : null));
        }
    }
}
