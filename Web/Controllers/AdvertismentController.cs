using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdvertismentController : ControllerBase
    {

        private readonly AdvertismentService _advertismentService;

        public AdvertismentController(
            AdvertismentService advertismentService
            ) 
        {
            _advertismentService = advertismentService;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType<AdvertismentDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(AdvertismentCreateDto dto)
        {
            var userId = HttpContext.GetUserId()!;
            return Ok(await _advertismentService.Create(userId.Value, dto));
        }

        [HttpGet]
        [ProducesResponseType<List<AdvertismentDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(
            string? query = null,
            Guid? categoryId = null,
            int skip = 0,
            int take = 5)
        {
            return Ok(await _advertismentService.Search(query, categoryId, skip, take));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<List<AdvertismentDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _advertismentService.GetMappedByIdAsync(id));
        }
    }
}
