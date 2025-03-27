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
using Microsoft.EntityFrameworkCore;
using HotelManagement.Models.Dtos.Delete;


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

        

       
        public async Task<HotelWitgReservationsDto> GetHotelByIdAsync(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);

            if (hotel == null)
            {
                throw new KeyNotFoundException($"Hotel with ID {id} not found");
            }
            

            return new HotelWitgReservationsDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Rating = hotel.Rating,
                Country = hotel.Country,
                City = hotel.City,
                Address = hotel.Address,
                Manager = hotel.Manager != null ? new ManagerDto
                {
                    Id = hotel.Manager.Id,
                    FirstName = hotel.Manager.FirstName,
                    LastName = hotel.Manager.LastName,
                    Email = hotel.Manager.Email,
                    MobileNumber = hotel.Manager.PhoneNumber
                } : null,
                Rooms = hotel.Rooms?.Select(r => new RoomWithReservationDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Price = r.Price,
                    IsAvailable = r.IsAvailable,
                    Reservations = r.Reservations?.Select(res => new ReservationShortDto
                    {
                        Id = res.Id,
                        CheckInDate = res.CheckInDate,
                        CheckOutDate = res.CheckOutDate,
                        GuestName = res.Guest != null ?
                            $"{res.Guest.FirstName} {res.Guest.LastName}" : "Unknown Guest",
                        GuestContact = res.Guest?.Email ?? res.Guest?.PhoneNumber ?? "N/A",
                        Status = res.IsAvailable ? "Active" : "Inactive"
                    }).ToList() ?? new List<ReservationShortDto>()
                }).ToList() ?? new List<RoomWithReservationDto>()
            };
        }


        public async Task<IEnumerable<HotelWitgReservationsDto>> GetFilteredHotelsAsync(string country, string city, int? rating)
        {

            var hotels = await _hotelRepository.GetFilteredHotelsAsync(
                country,
                city,
                rating,
                includeProperties: "Manager,Rooms,Reservations.Guest,Reservations.Room");

            if (!hotels.Any())
            {
                Console.WriteLine("No hotels found matching the criteria");
                return new List<HotelWitgReservationsDto>();
            }

            return hotels.Select(h => new HotelWitgReservationsDto
            {
                Id = h.Id,
                Name = h.Name,
                Rating = h.Rating,
                Country = h.Country,
                City = h.City,
                Address = h.Address,
                Manager = h.Manager != null ? new ManagerDto
                {
                    Id = h.Manager.Id,
                    FirstName = h.Manager.FirstName,
                    LastName = h.Manager.LastName,
                    Email = h.Manager.Email,
                    MobileNumber = h.Manager.PhoneNumber
                } : null,
                Rooms = h.Rooms?.Select(r => new RoomWithReservationDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Price = r.Price,
                    IsAvailable = r.IsAvailable,
                    Reservations = r.Reservations?.Select(res => new ReservationShortDto
                    {
                        Id = res.Id,
                        CheckInDate = res.CheckInDate,
                        CheckOutDate = res.CheckOutDate,
                        GuestName = res.Guest != null ?
                            $"{res.Guest.FirstName} {res.Guest.LastName}" : "Unknown Guest",
                        GuestContact = res.Guest?.Email ?? res.Guest?.PhoneNumber ?? "N/A"
                    }).ToList() ?? new List<ReservationShortDto>()
                }).ToList() ?? new List<RoomWithReservationDto>()
            }).ToList();
        }

        public async Task<DeleteResultDto> DeleteHotelAsync(int id)
        {
            try
            {
                bool success = await _hotelRepository.DeleteAsync(id);

                return success
                    ? new DeleteResultDto { Success = true, Message = "Hotel deleted successfully" }
                    : new DeleteResultDto { Success = false, Message = "Cannot delete hotel - it has active rooms or reservations" };
            }
            catch (Exception)
            {
                return new DeleteResultDto { Success = false, Message = "An error occurred while deleting the hotel" };
            }
        }
    }
}

