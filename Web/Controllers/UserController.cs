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

        public UserController(UserService userService) { _userService = userService; }

        [HttpPost("register")]
        [ProducesResponseType<TokenDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(UserRegistrationDto dto)
        {
            return Ok(await _userService.Register(dto));
        }


        [HttpPost("login")]
        [ProducesResponseType<TokenDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(UserAuthenticationDto dto)
        {
            return Ok(await _userService.Authenticate(dto));
        }

        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfile()
        {
            var id = HttpContext.GetUserId();
            return Ok(await _userService.GetProfile(id));
        }

        [Authorize]
        [HttpPatch("profile")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProfile(UserUpdateDto dto)
        {
            var id = HttpContext.GetUserId();
            return Ok(await _userService.UpdateProfile(id, dto));
        }

        [HttpGet]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllProfiles());
        }

        [HttpGet("{id}")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            return Ok(await _userService.GetProfile(id));
        }
    }
}
