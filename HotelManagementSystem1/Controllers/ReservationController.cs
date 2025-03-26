using HotelManagement.Models.Dtos.Reservations;
using HotelManagement.Service.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem1.Controllers
{
    [ApiController]
    [Route("api/hotel/reservations")]
    public class ReservationController : ControllerBase
    {

        private readonly IreservationService _service;
        public ReservationController(IreservationService service)
        {
            _service = service;
        }

        [HttpPost("create reservation")]
        public async Task<IActionResult> Create([FromBody] ReservationCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Reservation created successfully");
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ReservationUpdateDto dto)
        {
            await _service.UpdateAsync(dto);
            return Ok("Reservation updated successfully");
        }

        [HttpDelete("cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _service.CancelAsync(id);
            return Ok("Reservation cancelled successfully");
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] int? hotelId, [FromQuery] int? guestId, [FromQuery] int? roomId, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] bool? active)
        {
            var results = await _service.SearchAsync(hotelId, guestId, roomId, from, to, active);
            return Ok(results);
        }

    }
}
