using Microsoft.AspNetCore.Mvc;
using PosSystem.Application.Interfaces;
using PosSystem.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PosSystem.Api.Controllers
{
    [ApiController]
    [Route("api/Auth")]


   
    public class AuthControllers : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthControllers(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet("verify")]
        public IActionResult VerifySession()
        {
            // If the code reaches here, it means the JWT in the cookie is VALID.
            return Ok(new { 
                authenticated = true, 
                user = User.Identity?.Name,
                role = User.FindFirst(ClaimTypes.Role)?.Value
            });
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
        // Rename variable to 'result' to make it clear it's an object
        var result = await _authService.LoginAsync(loginDto.Email!, loginDto.Password!);
        
        if (!result.Success || result.Token == null)
        {
            return BadRequest("Login failed.");
        }else{

            var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // MUST be false for http://127.0.0.1
                    SameSite = SameSiteMode.Lax, // Lax is more forgiving for local dev
                    Expires = DateTime.UtcNow.AddDays(1),
                    Path = "/" // Ensure it is available for all paths
                };

            Response.Cookies.Append("jwt_token", result.Token, cookieOptions); 

            return Ok(new { success = true, message = "Logged in successfully", role = result.Role });
    }
    }


    }
}