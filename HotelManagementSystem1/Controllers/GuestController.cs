using System.Net;
using HotelManagement.Models.Dtos.Guest;
using HotelManagement.Service.Abstraction;
using HotelManagement.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllGuests()
        {
            var guests = await _service.GetAllAsync();
            return Ok(new ApiResponse("Guests retrieved successfully", guests, 200, true));
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Register([FromBody] GuestRegistrationDto dto)
        {
            await _service.RegisterAsync(dto);
            var response = new ApiResponse("Guest registered successfully", dto, 201, true);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] GuesUpdatingDto dto)
        {
            try
            {
                await _service.UpdateAsync(dto);
                var response = new ApiResponse("Guest updated successfully", dto, 200, true);
                return StatusCode(response.StatusCode, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(ex.Message, null, 400, false));
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var isDeleted = await _service.DeleteAsync(id);
            if (isDeleted)
            {
                return Ok(new ApiResponse(
                    message: "Guest deleted successfully",
                    data: new { GuestId = id },
                    statusCode: StatusCodes.Status200OK,
                    isSuccess: true
                ));
            }
            else
            {
                return BadRequest(new ApiResponse(
                    message: "Cannot delete guest - has active reservations or doesn't exist",
                    data: new { GuestId = id },
                    statusCode: StatusCodes.Status400BadRequest,
                    isSuccess: false
                ));
            }

        }
    }
}
