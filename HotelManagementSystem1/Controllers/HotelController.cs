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
        public async Task<IActionResult> CreateHotel([FromBody] HotelForCreatingDto dto)
        {
            await _hotelService.AddHotelAsync(dto);
            var response = new ApiResponse("სასტუმრო წარმატებით დაემატა", dto, 201, true);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateHotel([FromBody] HotelForUpdatingDto dto)
        {
            try
            {
                await _hotelService.UpdateHotelAsync(dto);
                var response = new ApiResponse("სასტუმრო განახლდა", dto, 200, true);
                return StatusCode(response.StatusCode, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(ex.Message, null, 400, false));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel([FromRoute] int id)
        {
            var result = await _hotelService.DeleteHotelAsync(id);
            if (!result)
                return BadRequest(new ApiResponse("სასტუმროს წაშლა შეუძლებელია, გააჩნია აქტიური ოთახები ან ჯავშნები.", null, 400, false));

            return StatusCode(204, new ApiResponse("წაშლა წარმატებით დასრულდა", null, 204, true));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById([FromRoute] int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
                return NotFound(new ApiResponse("სასტუმრო ვერ მოიძებნა", null, 404, false));

            return Ok(new ApiResponse("სასტუმრო მოიძებნა", hotel, 200, true));
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredHotels([FromQuery] string country, [FromQuery] string city, [FromQuery] int? rating)
        {
            var hotels = await _hotelService.GetFilteredHotelsAsync(country, city, rating);
            return Ok(new ApiResponse("სასტუმროების სია", hotels, 200, true));
        }
    }
}
