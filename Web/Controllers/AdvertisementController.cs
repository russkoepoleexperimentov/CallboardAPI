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
        private readonly CommentService _commentService;

        public AdvertisementController(
            AdvertisementService advertisementService,
            CommentService commentService
            ) 
        {
            _advertisementService = advertisementService;
            _commentService = commentService;
        }

        [HttpPost("search")]  // using POST because of advanced filtering with dictionaries
        [ProducesResponseType<List<AdvertisementDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(AdvertisementSearchDto dto)
        {
            return Ok(await _advertisementService.SearchMappedAsync(dto));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<List<AdvertisementDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _advertisementService.GetMappedAsync(id));
        }

        [Authorize]
        [HttpPost("{id}/image")]
        [ProducesResponseType<AdvertisementImageDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddImage(Guid id, ImageUploadDto dto)
        {
            var userId = HttpContext.GetUserId()!;
            return Ok(await _advertisementService.AddImageAsync(id, userId.Value, dto));
        }

        [Authorize]
        [HttpDelete("{id}/image/{imageId}")]
        [ProducesResponseType<AdvertisementImageDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveImage(Guid id, Guid imageId)
        {
            var userId = HttpContext.GetUserId()!;
            return Ok(await _advertisementService.RemoveImageAsync(id, userId.Value, imageId));
        }

        [HttpGet("{id}/comment")]
        [ProducesResponseType<List<CommentDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRootComments(Guid id)
        {
            return Ok(await _commentService.GetMappedRootsAsync(id));
        }

        [Authorize]
        [HttpPost("{id}/comment")]
        [ProducesResponseType<List<CommentDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateComment(CommentCreateUpdateDto dto, Guid id)
        {
            var userId = HttpContext.GetUserId()!;
            return Ok(await _commentService.CreateRootAndGetMappedAsync(dto, userId.Value, id));
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType<AdvertisementDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(AdvertisementCreateDto dto)
        {
            var userId = HttpContext.GetUserId()!;
            return Ok(await _advertisementService.CreateAndGetMappedAsync(userId.Value, dto));
        }

        [Authorize]
        [HttpPatch("{id}")]
        [ProducesResponseType<AdvertisementDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, AdvertisementUpdateDto dto)
        {
            var userId = HttpContext.GetUserId()!;
            return Ok(await _advertisementService.UpdateAndGetMappedAsync(id, userId.Value, dto));
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType<AdvertisementDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = HttpContext.GetUserId()!;
            return Ok(await _advertisementService.DeleteAndGetMappedAsync(id, userId.Value));
        }
    }
}
