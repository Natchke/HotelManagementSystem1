using HotelManagement.Models.Dtos.Room;
using HotelManagement.Service.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem1.Controllers
{
    [ApiController]
    [Route("api/Hotel/rooms")]
    [Authorize]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([FromBody] RoomForCreatingDto dto)
        {
            await _roomService.AddRoomAsync(dto);
            return Ok(new ApiResponse("Room created", dto, 201, true));
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update([FromBody] RoomForUpdatingDto dto)
        {
            await _roomService.UpdateRoomAsync(dto);
            return Ok(new ApiResponse("Room updated", dto, 200, true));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roomService.DeleteRoomAsync(id);
            if (!result)
                return BadRequest(new ApiResponse("Room has active bookings and cannot be deleted", null, 400, false));

            return Ok(new ApiResponse("Room deleted", null, 200, true));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room .Id== null)
                return NotFound(new ApiResponse("Room not found", null, 404, false));

            return Ok(new ApiResponse("Room found", room, 200, true));
        }


        [HttpGet]
        public async Task<IActionResult> Filter(
               [FromQuery] int? hotelId,
               [FromQuery] bool? isAvailable,
               [FromQuery] decimal? minPrice,
               [FromQuery] decimal? maxPrice)
        {
            var rooms = await _roomService.FilterRoomsAsync(hotelId, isAvailable, minPrice, maxPrice);
            return Ok(new ApiResponse("Rooms retrieved successfully", rooms, 200, true));
        }


    }
}
