using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Hotel;
using HotelManagement.Models.Dtos.Managerr;
using HotelManagement.Models.Dtos.Reservations;
using HotelManagement.Models.Dtos.Room;
using HotelManagement.Models.Entities;
using HotelManagement.Repository.Abstraction;
using HotelManagement.Service.Abstraction;

namespace HotelManagement.Service.Implementation
{
    public class HotelService : IHotelService

    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task AddHotelAsync(HotelForCreatingDto dto)
        {
            var hotel = new Hotel
            {
                Name = dto.Name,
                Address = dto.Address,
                Rating = dto.Rating,
                City = dto.City,
                Country = dto.Country,
                ManagerId = dto.ManagerId
            };

            await _hotelRepository.AddAsync(hotel);
        }

        public async Task UpdateHotelAsync(HotelForUpdatingDto dto)
        {
            if (dto.Rating < 1 || dto.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            var hotel = new Hotel
            {
                Id = dto.Id,
                Name = dto.Name,
                Address = dto.Address,
                Rating = dto.Rating
            };

            await _hotelRepository.UpdateAsync(hotel);
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            return await _hotelRepository.DeleteAsync(id);
        }

        public async Task<HotelDTO> GetHotelByIdAsync(int id)
        {
            var hotel = await _hotelRepository.GetAllAsync(h => h.Id == id, includeProperties: "Manager");


            if (hotel == null) throw new Exception("Hotel not found");

            // You can return hotel or map it to DTO
            return new HotelDTO
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Rating = hotel.Rating,
                Country = hotel.Country,
                City = hotel.City,
                Address = hotel.Address,
                ManagerId = hotel.ManagerId,
                Manager = new ManagerDto
                {
                    Id = hotel.Manager.Id,
                    FirstName = hotel.Manager.FirstName,
                    LastName = hotel.Manager.LastName,
                    Email = hotel.Manager.Email,
                    MobileNumber = hotel.Manager.MobileNumber
                }
            };
        }

        public async Task<IEnumerable<HotelDTO>> GetFilteredHotelsAsync(string country, string city, int? rating)
        {
            var hotels = await _hotelRepository.GetFilteredHotelsAsync(country, city, rating, includeProperties: "Manager,Rooms,Reservations");


            return hotels.Select(h => new HotelDTO
            {
                Id = h.Id,
                Name = h.Name,
                Rating = h.Rating,
                Country = h.Country,
                City = h.City,
                Address = h.Address,
                Manager = h.Manager == null ? null : new ManagerDto
                {
                    Id = h.Manager.Id,
                    FirstName = h.Manager.FirstName,
                    LastName = h.Manager.LastName,
                     Email = h.Manager.Email,
                    MobileNumber = h.Manager.MobileNumber
                },
                Rooms = h.Rooms?.Select(r => new RoomDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsAvailable = r.IsAvailable,
                    Price = r.Price
                }).ToList(),
                Reservations = h.Reservations?.Select(r => new ReservationDto
                {
                    Id = r.Id,
                    CheckInDate = r.CheckInDate,
                    CheckOutDate = r.CheckOutDate
                }).ToList()
            }).ToList();
        }

    }
}

