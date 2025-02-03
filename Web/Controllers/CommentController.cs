using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _service;

        public CommentController(CommentService commentService) 
        {
            _service = commentService;
        }


        [HttpGet("{id}")]
        [ProducesResponseType<CommentDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _service.GetMappedAsync(id));
        }


        [HttpGet("{id}/nested")]
        [ProducesResponseType<CommentDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNestedById(Guid id)
        {
            return Ok(await _service.GetMappedNestedAsync(id));
        }

        [Authorize]
        [HttpPost("{id}/reply")]
        [ProducesResponseType<List<CommentDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ReplyOnComment(CommentCreateUpdateDto dto, Guid id)
        {
            var userId = HttpContext.GetUserId()!;
            return Ok(await _service.ReplyAndGetMappedAsync(dto, userId.Value, id));
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType<CommentDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(CommentCreateUpdateDto dto, Guid id)
        {
            var userId = HttpContext.GetUserId();
            return Ok(await _service.UpdateAndGetMappedAsync(dto, id, userId!.Value));
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType<CommentDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = HttpContext.GetUserId();
            return Ok(await _service.DeleteAndGetMappedAsync(id, userId!.Value));
        }
    }
}
