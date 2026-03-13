using Microsoft.AspNetCore.Mvc;
using PosSystem.Application.Interfaces;
using PosSystem.Application.DTOs;


namespace PosSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthControllers : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthControllers(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto.Name!, registerDto.Email!, registerDto.Password!);
            if (result == null)
            {
                return BadRequest("Registration failed.");
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto.Email!, loginDto.Password!);
            if (result == null)
            {
                return BadRequest("Login failed.");
            }
            return Ok(result);
        }

    }
}