using Application.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Repositiories;

namespace Application.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper() 
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.ParentId,
                opt => opt.MapFrom((Func<Category, CategoryDto, Guid?>)
                ((s, d) => s.Parent != null ? s.Parent.Id : null)))
                .ForMember(dest => dest.NestedCount, opt => opt.Ignore());

            CreateMap<Category, CategoryFullDto>()
                .ForMember(dest => dest.ParentId,
                opt => opt.MapFrom((Func<Category, CategoryDto, Guid?>)
                ((s, d) => s.Parent != null ? s.Parent.Id : null)))
                .ForMember(dest => dest.NestedCount, opt => opt.Ignore())
                .ForMember(dest => dest.Parameters, opt => opt.Ignore());

            CreateMap<CategoryCreateDto, Category>()
                .ForMember(dest => dest.Parent, opt => opt.Ignore());
        }
    }
}
