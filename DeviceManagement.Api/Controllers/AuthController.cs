using DeviceManagement.Api.DTOs.Auth;
using DeviceManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DeviceManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService service, ILogger<AuthController> logger)
        {
            _authService = service;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequest)
        {
            var result = await _authService.LoginAsync(loginRequest);
            return Ok(result);
        }
        
        
    }
}
