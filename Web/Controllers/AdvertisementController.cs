using Application.DTOs;
using Application.Services;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdvertisementController : ControllerBase
    {

        private readonly AdvertisementService _advertisementService;

        public AdvertisementController(
            AdvertisementService advertisementService
            ) 
        {
            _advertisementService = advertisementService;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType<AdvertisementDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(AdvertisementCreateDto dto)
        {
            var userId = HttpContext.GetUserId()!;
            return Ok(await _advertisementService.CreateAndGetMappedAsync(userId.Value, dto));
        }

        [HttpGet("search")]
        [ProducesResponseType<List<AdvertisementDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(
            string? query = null,
            Guid? categoryId = null,
            AdvertisementSorting sorting = AdvertisementSorting.DateDesc,
            int skip = 0,
            int take = 5)
        {
            return Ok(await _advertisementService.SearchMappedAsync(query, categoryId, sorting, skip, take));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<List<AdvertisementDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _advertisementService.GetMappedAsync(id));
        }
    }
}
