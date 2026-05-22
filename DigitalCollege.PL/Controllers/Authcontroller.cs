using Microsoft.AspNetCore.Mvc;
using DigitalCollege.BLL.Interfaces;
using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;

namespace DigitalCollege.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var response = _authService.Authenticate(loginDto);
                return Ok(response);
            }
            catch (BusinessLogicException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("test-accounts")]
        public IActionResult GetTestAccounts()
        {
            return Ok(_authService.GetTestAccounts());
        }
    }
}
