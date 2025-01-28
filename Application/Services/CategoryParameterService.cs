
using Application.DTOs;
using AutoMapper;
using Common.Exceptions;
using Core.Entities;
using Core.Repositiories;

namespace Application.Services
{
    public class CategoryParameterService
    {
        private readonly ICategoryParameterRepository _categoryParameterRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryParameterService(
            ICategoryParameterRepository categoryParameterRepository, 
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryParameterRepository = categoryParameterRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryParameterDto> GetMappedAsync(Guid id)
        {
            var parameter = await GetParameterFromDbAsync(id);

            return _mapper.Map<CategoryParameter, CategoryParameterDto>(parameter);
        }

        public async Task<List<CategoryParameterDto>> GetMappedListForCategoryAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
                throw new NotFoundException("Category not found");

            var parameters = await _categoryParameterRepository.GetForCategoryAsync(categoryId);

            return parameters.Select(_mapper.Map<CategoryParameter, CategoryParameterDto>).ToList();
        }

        public async Task<CategoryParameterDto> CreateAndGetMappedAsync(Guid categoryId, CategoryParameterCreateDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
                throw new NotFoundException("Category not found");

            if (await _categoryRepository.NestedCount(category) > 0)
                throw new BadRequestException("Can't create parameters while nested category(-ies) exist");

            if (dto.DataType == Common.Enums.ParameterDataType.Enum && (
                dto.EnumValues == null ||
                dto.EnumValues.Count < 2
                ))
                throw new BadRequestException("Enum parameters must have at least 2 enum values");

            var parameter = _mapper.Map<CategoryParameterCreateDto, CategoryParameter>(dto);
            parameter.Category = category;

            await _categoryParameterRepository.AddAsync(parameter);

            return _mapper.Map<CategoryParameter, CategoryParameterDto>(parameter);
        }

        public async Task<CategoryParameterDto?> UpdateAndGetMappedAsync(Guid id, CategoryParameterUpdateDto dto)
        {
            var parameter = await GetParameterFromDbAsync(id);

            parameter.Name = dto.Name;
            await _categoryParameterRepository.UpdateAsync(parameter);
            return _mapper.Map<CategoryParameter, CategoryParameterDto>(parameter);
        }

        public async Task<CategoryParameterDto?> DeleteAndGetMappedAsync(Guid id)
        {
            var parameter = await GetParameterFromDbAsync(id);

            var dto = _mapper.Map<CategoryParameter, CategoryParameterDto>(parameter);
            await _categoryParameterRepository.DeleteAsync(id);
            return dto;
        }

        internal async Task<List<CategoryParameter>> GetParametersForCategoryFromDbAsync(Category category)
        {
            return await _categoryParameterRepository.GetForCategoryAsync(category.Id);
        }

        internal async Task<CategoryParameter> GetParameterFromDbAsync(Guid id)
        {
            var parameter = await _categoryParameterRepository.GetByIdAsync(id);

            if (parameter == null)
                throw new NotFoundException("Parameter not found");

            return parameter;
        }
    }
}
