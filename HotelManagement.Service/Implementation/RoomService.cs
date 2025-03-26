using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Room;
using HotelManagement.Models.Entities;
using HotelManagement.Repository.Abstraction;
using HotelManagement.Service.Abstraction;

namespace HotelManagement.Service.Implementation
{
    public class RoomService:IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task AddRoomAsync(RoomForCreatingDto dto)
        {
            var room = new Room
            {
                Name = dto.Name,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                HotelId = dto.HotelId
            };

            await _roomRepository.AddAsync(room);
        }

        public async Task UpdateRoomAsync(RoomForUpdatingDto dto)
        {
            var room = new Room
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable
            };

            await _roomRepository.UpdateAsync(room);
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            return await _roomRepository.DeleteAsync(id);
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _roomRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Room>> FilterRoomsAsync(int? hotelId, bool? isAvailable, decimal? minPrice, decimal? maxPrice)
        {
            return await _roomRepository.FilterRoomsAsync(hotelId, isAvailable, minPrice, maxPrice);
        }
    }
}
