using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Hotel;
using HotelManagement.Models.Dtos.Reservations;
using HotelManagement.Models.Dtos.Room;
using HotelManagement.Models.Entities;
using HotelManagement.Repository.Abstraction;
using HotelManagement.Repository.Implementation;
using HotelManagement.Service.Abstraction;

namespace HotelManagement.Service.Implementation
{
    public class RoomService:IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;

        public RoomService(IRoomRepository roomRepository,IHotelRepository hotelRepository)
        {
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
        }

        public async Task AddRoomAsync(RoomForCreatingDto dto)
        {
            // Add validation to check if hotel exists
            var hotelExists = await _hotelRepository.GetByIdAsync(dto.HotelId)!=null;
            if (!hotelExists)
                throw new ArgumentException("Specified hotel does not exist");

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

        public async Task<RoomDto> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id, includeHotel: true, includeReservations: true);

            if (room == null) throw new KeyNotFoundException($"Room with ID {id} not found");

            return new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Price = room.Price,
                IsAvailable = room.IsAvailable,
                HotelId = room.HotelId,
                Hotel = room.Hotel != null ? new HotelShortInfoDto
                {
                    Id = room.Hotel.Id,
                    Name = room.Hotel.Name,
                    Rating = room.Hotel.Rating,
                } : null,
                Reservations = room.Reservations?
                    .Select(r => new ReservationDto
                    {
                        Id = r.Id,
                        CheckInDate = r.CheckInDate,
                        CheckOutDate = r.CheckOutDate,
                        GuestName = r.Guest != null ?
                            $"{r.Guest.FirstName} {r.Guest.LastName}" :
                            "Guest"
                    })
                    .OrderBy(r => r.CheckInDate)
                    .ToList() ?? new List<ReservationDto>()
            };
        }

        public async Task<IEnumerable<RoomDto>> FilterRoomsAsync(int? hotelId, bool? isAvailable, decimal? minPrice, decimal? maxPrice)
        {
            var rooms = await _roomRepository.FilterRoomsAsync(hotelId, isAvailable, minPrice, maxPrice);

            return rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Price = r.Price,
                IsAvailable = r.IsAvailable,
                HotelId = r.HotelId,
                Hotel = r.Hotel != null ? new HotelShortInfoDto
                {
                    Id = r.Hotel.Id,
                    Name = r.Hotel.Name,
                    Rating = r.Hotel.Rating
                } : null,
                Reservations = r.Reservations?.Select(res => new ReservationDto
                {
                    Id = res.Id,
                    CheckInDate = res.CheckInDate,
                    CheckOutDate = res.CheckOutDate,
                    GuestName = res.Guest != null ?
                            $"{res.Guest.FirstName} {res.Guest.LastName}" :
                            "Guest"

                }).ToList() ?? new List<ReservationDto>()
            });
        }
    }
}
