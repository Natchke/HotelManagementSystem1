using System.Net;
using HotelManagement.Models.Dtos.Identity;
using HotelManagement.Models.Dtos.Manager;
using HotelManagement.Service.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem1.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse == null)
            {
                return Unauthorized(new ApiResponse("Invalid username or password", null, 401, false));
            }

            var response = new ApiResponse("Login successful", loginResponse, 200, true);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            await _authService.RegisterUser(model);
            var response = new ApiResponse("User registered successfully", model, 201, true);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("registeradmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegistrationDto model)
        {
            await _authService.RegisterAdmin(model);
            var response = new ApiResponse("Admin registered successfully", model, 201, true);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("registermanager")]
        public async Task<IActionResult> RegisterManager([FromBody] ManagerRegistrationDto model)
        {
            await _authService.RegisterManager(model);
            var response = new ApiResponse("Manager registered successfully", model, 201, true);
            return StatusCode(response.StatusCode, response);
        }
    }
}
