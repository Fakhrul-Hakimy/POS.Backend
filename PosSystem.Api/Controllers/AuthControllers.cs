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

       
    }
}