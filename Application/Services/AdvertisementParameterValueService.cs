
using Application.DTOs;
using AutoMapper;
using Common.Enums;
using Common.Exceptions;
using Core.Entities;
using Core.Repositiories;
using System.Text.Json;

namespace Application.Services
{
    public class AdvertisementParameterValueService
    {
        private readonly IAdvertisementParameterValueRepository _repository;
        private readonly IMapper _mapper;
        private readonly CategoryParameterService _categoryParameterService;

        public AdvertisementParameterValueService(
            IAdvertisementParameterValueRepository repository,
            IMapper mapper,
            CategoryParameterService categoryParameterService
            )
        {
            _repository = repository;
            _mapper = mapper;
            _categoryParameterService = categoryParameterService;
        }

        public async Task<List<AdvertisementParameterValueDto>> GetAllForAdvertisementAsync(Advertisement advertisement)
        {
            return (await _repository.GetForAdvertisment(advertisement))
                .Select(_mapper.Map<AdvertisementParameterValue, AdvertisementParameterValueDto>)
                .ToList();
        }

        internal async Task<List<AdvertisementParameterValue>> CreateAndGetListAsync(List<AdvertisementParameterValueCreateDto> list)
        {
            List<AdvertisementParameterValue> result = new();
            foreach (var item in list)
                result.Add(await CreateAndGetAsync(item));
            return result;
        }

        internal async Task<AdvertisementParameterValue> CreateAndGetAsync(AdvertisementParameterValueCreateDto dto)
        {
            var element = dto.Value;
            var needType = (await _categoryParameterService.GetParameterFromDbAsync(dto.ParameterId)).DataType;
            var result = _mapper.Map<AdvertisementParameterValueCreateDto, AdvertisementParameterValue>(dto);

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

            result.CategoryParameter = await _categoryParameterService.GetParameterFromDbAsync(dto.ParameterId);

            return result;
        }

        internal async Task AddListToDbAsync(Advertisement advertisment, List<AdvertisementParameterValue> parameterValues)
        {
            foreach (var parameterValue in parameterValues)
            {
                parameterValue.Advertisment = advertisment;
                await _repository.AddAsync(parameterValue);
            }
        }
    }
}
