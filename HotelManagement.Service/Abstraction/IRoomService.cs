using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Room;
using HotelManagement.Models.Entities;

namespace HotelManagement.Service.Abstraction
{
    public interface IRoomService 
    {
        Task AddRoomAsync(RoomForCreatingDto dto);
        Task UpdateRoomAsync(RoomForUpdatingDto dto);
        Task<bool> DeleteRoomAsync(int id);
        Task<RoomDto> GetRoomByIdAsync(int id);
        Task<IEnumerable<RoomDto>> FilterRoomsAsync(int? hotelId, bool? isAvailable, decimal? minPrice, decimal? maxPrice);

    }
}
