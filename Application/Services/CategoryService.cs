
using Application.DTOs;
using AutoMapper;
using Common.Exceptions;
using Core.Entities;
using Core.Repositiories;

namespace Application.Services
{
    public class CategoryService
    {
        private readonly CategoryParameterService _parameterService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _categoryMapper;

        public CategoryService(
            CategoryParameterService parameterService,
            ICategoryRepository categoryRepository,
            IMapper categoryMapper)
        {
            _parameterService = parameterService;
            _categoryRepository = categoryRepository;
            _categoryMapper = categoryMapper;
        }

        public async Task<CategoryDto> CreateAndGetMappedAsync(CategoryCreateDto dto)
        {
            var category = _categoryMapper.Map<CategoryCreateDto, Category>(dto);

            if(dto.ParentId != null)
            {
                var parent = await _categoryRepository.GetByIdAsync(dto.ParentId.Value);

                if (parent == null)
                    throw new NotFoundException("Parent category not found");

                if ((await _parameterService.GetMappedListForCategoryAsync(parent.Id)).Count > 0)
                    throw new BadRequestException("Unnable to create nested for parent with parameters.");

                category.Parent = parent;
            }

            await _categoryRepository.AddAsync(category);

            return _categoryMapper.Map<Category, CategoryDto>(category);
        }

        public async Task<CategoryFullDto> GetMappedAsync(Guid id)
        {
            return await GetFullMappedFromAsync(await GetFromDbAsync(id));
        }

        public async Task<List<CategoryDto>> GetMappedNestedListAsync(Guid id)
        {
            return (await GetNestedFromDbAsync(id, false)).Select(GetMappedFrom).ToList();
        }

        public async Task<List<CategoryDto>> GetMappedRootsAsync()
        {
            var roots = await _categoryRepository.GetRoots();

            return roots.Select(GetMappedFrom).ToList();
        }

        internal CategoryDto GetMappedFrom(Category category)
        {
            var dto = _categoryMapper.Map<Category, CategoryDto>(category);
            var task = _categoryRepository.NestedCount(category);
            task.Wait();
            dto.NestedCount = task.Result;
            return dto;
        }

        internal async Task<CategoryFullDto> GetFullMappedFromAsync(Category category)
        {
            var dto = _categoryMapper.Map<Category, CategoryFullDto>(category);
            var task = _categoryRepository.NestedCount(category);
            task.Wait();
            dto.NestedCount = task.Result;
            dto.Parameters = await _parameterService.GetMappedListForCategoryAsync(category.Id);
            return dto;
        }

        internal async Task<Category> GetFromDbAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new NotFoundException("Category not found");

            return category;
        }

        internal async Task<List<Category>> GetNestedFromDbAsync(Guid id, bool inclusive = false)
        {
            var category = await GetFromDbAsync(id);
            var nested = await _categoryRepository.GetNested(category);
            if(inclusive)
                nested.Add(category);
            return nested;
        }
    }
}
