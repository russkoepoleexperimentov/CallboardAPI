using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly CategoryParameterService _categoryParameterService;

        public CategoryController(CategoryService service, CategoryParameterService parameterService) 
        { 
            _categoryService = service;
            _categoryParameterService = parameterService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType<CategoryFullDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetById(Guid id)
        {
            return Ok(await _categoryService.GetById(id));
        }

        [HttpGet("{id}/nested")]
        [ProducesResponseType<List<CategoryDto>>(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetNested(Guid id)
        {
            return Ok(await _categoryService.GetNested(id));
        }

        [HttpGet("roots")]
        [ProducesResponseType<List<CategoryDto>>(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetRoots()
        {
            return Ok(await _categoryService.GetRoots());
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType<CategoryDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult> Create(CategoryCreateDto dto)
        {
            return Ok(await _categoryService.Create(dto));
        }

        [HttpPost("{id}/addParameter")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType<CategoryParameterDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult> CreateParameter(Guid id, CategoryParameterCreateDto dto)
        {
            return Ok(await _categoryParameterService.Create(id, dto));
        }
    }
}
