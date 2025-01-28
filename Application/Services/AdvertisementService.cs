using Application.DTOs;
using AutoMapper;
using Common.Enums;
using Common.Exceptions;
using Core.Entities;
using Core.Repositiories;

namespace Application.Services
{
    public class AdvertisementService
    {
        private readonly UserService _userService;
        private readonly CategoryService _categoryService;
        private readonly CategoryParameterService _categoryParameterService;
        private readonly AdvertisementParameterValueService _advertismentParameterValueService;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IMapper _mapper;

        public AdvertisementService(
            IAdvertisementRepository advertisementRepository,
            UserService userService,
            CategoryService categoryService,
            CategoryParameterService categoryParameterService,
            AdvertisementParameterValueService advertismentParameterValueService,
            IMapper mapper
            )
        {
            _advertisementRepository = advertisementRepository;
            _userService = userService;
            _categoryService = categoryService;
            _categoryParameterService = categoryParameterService;
            _advertismentParameterValueService = advertismentParameterValueService;
            _mapper = mapper;
        }

        public async Task<List<AdvertisementDto>> SearchMappedAsync
            (
            string? query = null,
            Guid? categoryId = null,
            AdvertisementSorting sorting = AdvertisementSorting.DateAsc,
            int skip = 0,
            int take = 5
            )
        {
            var categories = categoryId != null ?
                (await _categoryService.GetNestedFromDbAsync(categoryId.Value)).Select(x => x.Id).ToList() :
                null;

            var result = await _advertisementRepository.Search(query, categories, sorting, skip, take);
            List<AdvertisementDto> dtos = new();

            foreach (var item in result) 
                dtos.Add(await GetMappedAsync(item));

            return dtos;
        }

        public async Task<AdvertisementDto> GetMappedAsync(Guid id)
        {
            return await GetMappedAsync(await GetFromDbAsync(id));
        }

        public async Task<AdvertisementDto> CreateAndGetMappedAsync(
            Guid userId, 
            AdvertisementCreateDto dto
            )
        {
            var user = await _userService.GetFromDbAsync(userId);
            var category = await _categoryService.GetFromDbAsync(dto.CategoryId);
            var parameters = await _categoryParameterService.GetParametersForCategoryFromDbAsync(category);
            var parametersCheck = parameters.Select(x => x.Id).ToDictionary(x => x, x => false);

            if (parameters.Count != dto.Parameters.Count)
                throw new BadRequestException("Parameters count mismatch");

            foreach (var parameter in dto.Parameters)
            {
                if(!parametersCheck.ContainsKey(parameter.ParameterId))
                    throw new BadRequestException($"Invalid parameter {parameter.ParameterId}");

                if (parametersCheck[parameter.ParameterId] == true)
                    throw new BadRequestException($"Parameter duplication {parameter.ParameterId}");
            }

            var parameterValues = await _advertismentParameterValueService.CreateAndGetListAsync(dto.Parameters);

            var advertisement = _mapper.Map<AdvertisementCreateDto, Advertisement>(dto);
            advertisement.Author = user;
            advertisement.Category = category;

            await _advertisementRepository.AddAsync(advertisement);
            await _advertismentParameterValueService.AddListToDbAsync(advertisement, parameterValues);

            return await GetMappedAsync(advertisement);
        }

        internal async Task<Advertisement> GetFromDbAsync(Guid id)
        {
            var item = await _advertisementRepository.GetByIdAsync(id);

            if(item == null)
                throw new NotFoundException("Advertisment not found");

            return item;
        }

        private async Task<AdvertisementDto> GetMappedAsync(Advertisement advertisment)
        {
            var result = _mapper.Map<Advertisement, AdvertisementDto>(advertisment);
            result.Category = await _categoryService.GetFullMappedFromAsync(advertisment.Category);
            result.Parameters = await _advertismentParameterValueService.GetAllForAdvertisementAsync(advertisment);
            return result;
        }
    }
}
