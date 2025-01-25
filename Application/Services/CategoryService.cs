
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

        public async Task<CategoryDto> Create(CategoryCreateDto dto)
        {
            var category = _categoryMapper.Map<CategoryCreateDto, Category>(dto);

            if(dto.ParentId != null)
            {
                var parent = await _categoryRepository.GetByIdAsync(dto.ParentId.Value);

                if (parent == null)
                    throw new NotFoundException("Parent category not found");

                if ((await _parameterService.GetForCategory(parent.Id)).Count > 0)
                    throw new BadRequestException("Unnable to create nested for parent with parameters.");

                category.Parent = parent;
            }

            await _categoryRepository.AddAsync(category);

            return _categoryMapper.Map<Category, CategoryDto>(category);
        }

        public async Task<CategoryFullDto> GetById(Guid id)
        {
            return await FullMapFrom(await GetCategoryAsync(id));
        }

        public async Task<List<CategoryDto>> GetNested(Guid id)
        {
            return (await GetNestedAsync(id, false)).Select(MapFrom).ToList();
        }

        public async Task<List<CategoryDto>> GetRoots()
        {
            var roots = await _categoryRepository.GetRoots();

            return roots.Select(MapFrom).ToList();
        }

        internal CategoryDto MapFrom(Category category)
        {
            var dto = _categoryMapper.Map<Category, CategoryDto>(category);
            var task = _categoryRepository.NestedCount(category);
            task.Wait();
            dto.NestedCount = task.Result;
            return dto;
        }

        internal async Task<CategoryFullDto> FullMapFrom(Category category)
        {
            var dto = _categoryMapper.Map<Category, CategoryFullDto>(category);
            var task = _categoryRepository.NestedCount(category);
            task.Wait();
            dto.NestedCount = task.Result;
            dto.Parameters = await _parameterService.GetForCategory(category.Id);
            return dto;
        }

        internal async Task<Category> GetCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new NotFoundException("Category not found");

            return category;
        }

        internal async Task<List<Category>> GetNestedAsync(Guid id, bool inclusive = false)
        {
            var category = await GetCategoryAsync(id);
            var nested = await _categoryRepository.GetNested(category);
            if(inclusive)
                nested.Add(category);
            return nested;
        }
    }
}
