using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ImageService _imageService;

        public ImageController(
            ImageService imageService
            )
        {
            _imageService = imageService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ShowImage(Guid id)
        {
            var dto = await _imageService.GetAsync(id);
            return File(dto.Data, dto.ContentType);
        }
    }
}
