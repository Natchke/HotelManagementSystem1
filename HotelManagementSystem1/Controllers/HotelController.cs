using HotelManagement.Models.Dtos.Hotel;
using HotelManagement.Models.Entities;
using HotelManagement.Service.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace HotelManagementSystem1.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateHotel([FromBody] HotelForCreatingDto dto)
        {
            await _hotelService.AddHotelAsync(dto);
            var response = new ApiResponse("Hotel Added Successfuly", dto, 201, true);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateHotel([FromBody] HotelForUpdatingDto dto)
        {
            try
            {
                await _hotelService.UpdateHotelAsync(dto);
                var response = new ApiResponse("Hotel Updated ", dto, 200, true);
                return StatusCode(response.StatusCode, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(ex.Message, null, 400, false));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var result = await _hotelService.DeleteHotelAsync(id);
            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetHotelById([FromRoute] int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
                return NotFound(new ApiResponse("Hotel Not Found", null, 404, false));

            return Ok(new ApiResponse("Hotel Found", hotel, 200, true));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFilteredHotels([FromQuery] string country, [FromQuery] string city, [FromQuery] int? rating)
        {
            var hotels = await _hotelService.GetFilteredHotelsAsync(country, city, rating);
            return Ok(new ApiResponse("List Of Hotels", hotels, 200, true));
        }
    }
}
