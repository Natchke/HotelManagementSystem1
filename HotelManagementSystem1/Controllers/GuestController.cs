using HotelManagement.Models.Dtos.Guest;
using HotelManagement.Service.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem1.Controllers
{
    [ApiController]
    [Route("api/hotel/guests")]
    public class GuestController : ControllerBase
    {
        private readonly IGuestService _service;
        public GuestController(IGuestService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] GuestRegistrationDto dto)
        {
            await _service.RegisterAsync(dto);
            return Ok("Guest registered successfully");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] GuesUpdatingDto dto)
        {
            await _service.UpdateAsync(dto);
            return Ok("Guest updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Guest deleted successfully");
        }

    }
}
