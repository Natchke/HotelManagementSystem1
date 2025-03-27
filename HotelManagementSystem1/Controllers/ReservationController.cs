using HotelManagement.Models.Dtos.Reservations;
using HotelManagement.Service.Abstraction;
using HotelManagement.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem1.Controllers
{
    [ApiController]
    [Route("api/hotels/reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IreservationService _service;

        public ReservationController(IreservationService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationCreateDto dto)
        {
            await _service.CreateAsync(dto);
            var response = new ApiResponse("Reservation created successfully.", dto, 201, true);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateReservation([FromBody] ReservationUpdateDto dto)
        {
            try
            {
                await _service.UpdateAsync(dto);
                var response = new ApiResponse("Reservation updated successfully.", dto, 200, true);
                return StatusCode(response.StatusCode, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(ex.Message, null, 400, false));
            }
        }


        [HttpDelete("cancel/{id}")]
        [Authorize]
        public async Task<IActionResult> Cancel(int id)
        {
            await _service.CancelAsync(id);
            return Ok("Reservation cancelled successfully");
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> SearchReservations([FromQuery] int? hotelId, 
                                                           [FromQuery] string guestId, 
                                                           [FromQuery] int? roomId, 
                                                           [FromQuery] DateTime? from, 
                                                           [FromQuery] DateTime? to, 
                                                           [FromQuery] bool? active)
        {
            var results = await _service.SearchAsync(hotelId, guestId, roomId, from, to, active);
            return Ok(new ApiResponse("Reservations retrieved successfully.", results, 200, true));
        }
    }
}
