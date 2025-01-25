
using Application.DTOs;
using AutoMapper;
using Common.Enums;
using Common.Exceptions;
using Core.Entities;
using Core.Repositiories;
using System.Text.Json;

namespace Application.Services
{
    public class AdvertismentParameterValueService
    {
        private readonly IAdvertismentParameterValueRepository _repository;
        private readonly IMapper _mapper;
        private readonly CategoryParameterService _categoryParameterService;

        public AdvertismentParameterValueService(
            IAdvertismentParameterValueRepository repository,
            IMapper mapper,
            CategoryParameterService categoryParameterService
            )
        {
            _repository = repository;
            _mapper = mapper;
            _categoryParameterService = categoryParameterService;
        }

        public async Task<List<AdvertismentParameterValueDto>> GetForAdvertisment(Advertisment advertisment)
        {
            return (await _repository.GetForAdvertisment(advertisment))
                .Select(_mapper.Map<AdvertismentParameterValue, AdvertismentParameterValueDto>)
                .ToList();
        }

        internal async Task<List<AdvertismentParameterValue>> CreateList(List<AdvertismentParameterValueCreateDto> list)
        {
            List<AdvertismentParameterValue> result = new();
            foreach (var item in list)
                result.Add(await Create(item));
            return result;
        }

        internal async Task<AdvertismentParameterValue> Create(AdvertismentParameterValueCreateDto dto)
        {
            var element = (JsonElement)dto.Value;
            var needType = (await _categoryParameterService.GetParameterAsync(dto.ParameterId)).DataType;
            var result = _mapper.Map<AdvertismentParameterValueCreateDto, AdvertismentParameterValue>(dto);

            try
            {

                switch (needType)
                {
                    case ParameterDataType.Integer:
                        result.IntegerValue = element.GetInt32();
                        break;
                    case ParameterDataType.Float:
                        result.FloatValue = element.GetSingle();
                        break;
                    case ParameterDataType.String:
                        result.StringValue = element.GetString();
                        break;
                    case ParameterDataType.Boolean:
                        result.BooleanValue = element.GetBoolean();
                        break;
                    case ParameterDataType.Enum:
                        result.EnumValue = element.GetInt32();
                        break;

                }
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Parameter {dto.ParameterId} value type mismatch. {ex.Message}");
            }

            result.CategoryParameter = await _categoryParameterService.GetParameterAsync(dto.ParameterId);

            return result;
        }

        internal async Task AddListAsync(Advertisment advertisment, List<AdvertismentParameterValue> parameterValues)
        {
            foreach (var parameterValue in parameterValues)
            {
                parameterValue.Advertisment = advertisment;
                await _repository.AddAsync(parameterValue);
            }
        }
    }
}
