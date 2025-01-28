using Application.DTOs;
using Application.Services;
using Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(
            UserService userService
            ) 
        {
            _userService = userService; 
        }

        [HttpPost("register")]
        [ProducesResponseType<TokenDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(UserRegistrationDto dto)
        {
            return Ok(await _userService.RegisterAndGetTokenAsync(dto));
        }


        [HttpPost("login")]
        [ProducesResponseType<TokenDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(UserAuthenticationDto dto)
        {
            return Ok(await _userService.AuthenticateAndGetTokenAsync(dto));
        }

        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfile()
        {
            var id = HttpContext.GetUserId();
            return Ok(await _userService.GetMappedAsync(id));
        }

        [Authorize]
        [HttpPost("profile/avatar")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadAvatar([FromForm] ImageUploadDto dto)
        {
            var id = HttpContext.GetUserId();
            return Ok(await _userService.UploadAvatarAsync(id, dto));
        }

        [Authorize]
        [HttpPatch("profile")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProfile(UserUpdateDto dto)
        {
            var id = HttpContext.GetUserId();
            return Ok(await _userService.UpdateAndGetMappedAsync(id, dto));
        }

        [HttpGet]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllMappedAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            return Ok(await _userService.GetMappedAsync(id));
        }
    }
}
