
using Application.DTOs;
using AutoMapper;
using Common.Enums;
using Core.Entities;

namespace Application.Mappers
{
    public class AdvertismentParameterValueMapper : Profile
    {
        public AdvertismentParameterValueMapper() 
        {
            CreateMap<AdvertismentParameterValue, AdvertismentParameterValueDto>()
                .ForMember(x => x.ParameterId, o => o.MapFrom(x => x.CategoryParameter.Id))
                .ForMember(x => x.Name, o => o.MapFrom(x => x.CategoryParameter.Name))
                .ForMember(x => x.DataType, o => o.MapFrom(x => x.CategoryParameter.DataType))
                .ForMember(x => x.Value, o => o.MapFrom(x => GetValue(x)));

            CreateMap<AdvertismentParameterValueCreateDto, AdvertismentParameterValue>()
                .ForMember(x => x.Advertisment, o => o.Ignore())
                .ForMember(x => x.CategoryParameter, o => o.Ignore())
                .ForMember(x => x.EnumValue, o => o.Ignore())
                .ForMember(x => x.IntegerValue, o => o.Ignore())
                .ForMember(x => x.FloatValue, o => o.Ignore())
                .ForMember(x => x.StringValue, o => o.Ignore())
                .ForMember(x => x.BooleanValue, o => o.Ignore());
        }

        private object? GetValue(AdvertismentParameterValue source)
        {
            var dtype = source.CategoryParameter.DataType;

            switch (dtype)
            {
                case ParameterDataType.Integer:
                    return source.IntegerValue!;
                case ParameterDataType.Float:
                    return source.FloatValue!;
                case ParameterDataType.Boolean:
                    return source.BooleanValue!;
                case ParameterDataType.String:
                    return source.StringValue!;
                case ParameterDataType.Enum:
                    return source.EnumValue!;
                default:
                    return null;
            }
        }
    }
}
