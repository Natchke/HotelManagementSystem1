using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Entities;

namespace HotelManagement.Repository.Abstraction
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task AddAsync(Room room);
        Task UpdateAsync(Room room);
        Task<bool> DeleteAsync(int roomId);
        Task<Room> GetByIdAsync(int id);
        Task<IEnumerable<Room>> FilterRoomsAsync(int? hotelId, bool? isAvailable, decimal? minPrice, decimal? maxPrice);
    }
}
