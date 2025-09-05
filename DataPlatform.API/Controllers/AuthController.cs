using DataPlatform.Application.Interfaces;
using DataPlatform.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DataPlatform.API.Controllers
{
    [Route("auth")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDto);
            
            if (result == null)
                return Unauthorized("Invalid username or password");

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var sessionToken = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            
            if (string.IsNullOrEmpty(sessionToken))
                return BadRequest("Session token required");

            await _authService.LogoutAsync(sessionToken);
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var sessionToken = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            
            if (string.IsNullOrEmpty(sessionToken))
                return Unauthorized("Session token required");

            var user = await _authService.GetUserBySessionAsync(sessionToken);
            
            if (user == null)
                return Unauthorized("Invalid session");

            return Ok(user);
        }
    }
}