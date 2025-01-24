using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryParameterController : ControllerBase
    {
        private readonly CategoryParameterService _categoryParameterService;

        public CategoryParameterController(CategoryParameterService categoryParameterService)
        {
            _categoryParameterService = categoryParameterService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType<CategoryParameterDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetParameter(Guid id)
        {
            return Ok(await _categoryParameterService.GetById(id));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType<CategoryParameterDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateParameter(Guid id, CategoryParameterUpdateDto dto)
        {
            return Ok(await _categoryParameterService.Update(id, dto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType<CategoryParameterDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteParameter(Guid id)
        {
            return Ok(await _categoryParameterService.Delete(id));
        }
    }
}
