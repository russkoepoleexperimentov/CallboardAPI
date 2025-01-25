using Application.DTOs;
using AutoMapper;
using Common.Enums;
using Common.Exceptions;
using Core.Entities;
using Core.Repositiories;

namespace Application.Services
{
    public class AdvertismentService
    {
        private readonly UserService _userService;
        private readonly CategoryService _categoryService;
        private readonly CategoryParameterService _categoryParameterService;
        private readonly AdvertismentParameterValueService _advertismentParameterValueService;
        private readonly IAdvertismentRepository _advertismentRepository;
        private readonly IMapper _mapper;

        public AdvertismentService(
            IAdvertismentRepository advertismentRepository,
            UserService userService,
            CategoryService categoryService,
            CategoryParameterService categoryParameterService,
            AdvertismentParameterValueService advertismentParameterValueService,
            IMapper mapper
            )
        {
            _advertismentRepository = advertismentRepository;
            _userService = userService;
            _categoryService = categoryService;
            _categoryParameterService = categoryParameterService;
            _advertismentParameterValueService = advertismentParameterValueService;
            _mapper = mapper;
        }

        public async Task<List<AdvertismentDto>> Search
            (
            string? query = null,
            Guid? categoryId = null,
            int skip = 0,
            int take = 5
            )
        {
            var categories = categoryId != null ?
                (await _categoryService.GetNestedAsync(categoryId.Value)).Select(x => x.Id).ToList() :
                null;

            var result = await _advertismentRepository.Search(query, categories, skip, take);
            List<AdvertismentDto> dtos = new();

            foreach (var item in result) 
                dtos.Add(await MapAdvetrismentAsync(item));

            return dtos;
        }

        public async Task<AdvertismentDto> GetMappedByIdAsync(Guid id)
        {
            return await MapAdvetrismentAsync(await GetByIdAsync(id));
        }

        public async Task<AdvertismentDto> Create(
            Guid userId, 
            AdvertismentCreateDto dto
            )
        {
            var user = await _userService.GetUser(userId);
            var category = await _categoryService.GetCategoryAsync(dto.CategoryId);
            var parameters = await _categoryParameterService.GetParametersForCategoryAsync(category);
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

            var parameterValues = await _advertismentParameterValueService.CreateList(dto.Parameters);

            var advertisment = _mapper.Map<AdvertismentCreateDto, Advertisment>(dto);
            advertisment.Author = user;
            advertisment.Category = category;

            await _advertismentRepository.AddAsync(advertisment);
            await _advertismentParameterValueService.AddListAsync(advertisment, parameterValues);

            return await MapAdvetrismentAsync(advertisment);
        }

        internal async Task<Advertisment> GetByIdAsync(Guid id)
        {
            var item = await _advertismentRepository.GetByIdAsync(id);

            if(item == null)
                throw new NotFoundException("Advertisment not found");

            return item;
        }

        private async Task<AdvertismentDto> MapAdvetrismentAsync(Advertisment advertisment)
        {
            var result = _mapper.Map<Advertisment, AdvertismentDto>(advertisment);
            result.Category = await _categoryService.FullMapFrom(advertisment.Category);
            result.Parameters = await _advertismentParameterValueService.GetForAdvertisment(advertisment);
            return result;
        }
    }
}
