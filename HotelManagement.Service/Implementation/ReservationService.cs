using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Reservations;
using HotelManagement.Models.Entities;
using HotelManagement.Repository.Abstraction;
using HotelManagement.Repository.Data;
using HotelManagement.Repository.Implementation;
using HotelManagement.Service.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Service.Implementation
{
    public class ReservationService : IreservationService
    {
        private readonly IReservationRepository _repo;
        private readonly ApplicationDbContext _context;
        public ReservationService(IReservationRepository repo, ApplicationDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task CreateAsync(ReservationCreateDto dto)
        {
            if (!await _repo.IsRoomAvailable(dto.RoomId, dto.CheckInDate, dto.CheckOutDate))
                throw new Exception("Room is not available for the selected dates");

            if (dto.CheckInDate.Date < DateTime.Today || dto.CheckInDate.Date > DateTime.Today.AddDays(1))
                throw new Exception("Check-in date must be today or tomorrow");

            if (dto.CheckOutDate <= dto.CheckInDate)
                throw new Exception("Check-out date must be after check-in");

            var room = await _context.Rooms.FindAsync(dto.RoomId);
            if (room == null || !room.IsAvailable)
                throw new Exception("Room is not available");

            room.IsAvailable = false;

            var reservation = new Reservation
            {
                RoomId = dto.RoomId,
                GuestId = dto.GuestId,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate
            };

            await _repo.AddAsync(reservation);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(ReservationUpdateDto dto)
        {
            var reservation = await _repo.GetByIdAsync(dto.Id);
            if (reservation == null) throw new Exception("Reservation not found");

            if (!await _repo.IsRoomAvailable(reservation.RoomId, dto.NewCheckInDate, dto.NewCheckOutDate))
                throw new Exception("Room is not available for new dates");

            reservation.CheckInDate = dto.NewCheckInDate;
            reservation.CheckOutDate = dto.NewCheckOutDate;

            await _repo.SaveAsync();
        }

        public async Task CancelAsync(int id)
        {
            var reservation = await _repo.GetByIdAsync(id);
            if (reservation == null) throw new Exception("Reservation not found");

            var room = await _context.Rooms.FindAsync(reservation.RoomId);
            if (room != null)
                room.IsAvailable = true;

            _repo.Remove(reservation);
            await _repo.SaveAsync();
        }

        public async Task<IEnumerable<ReservationDto>> SearchAsync(int? hotelId, string guestId, int? roomId, DateTime? from, DateTime? to, bool? active)
        {
           
            var query = _context.Reservations
                .Include(r => r.Guest)  
                .Include(r => r.Room)   
                    .ThenInclude(room => room.Hotel)  
                .AsQueryable();

            
            if (hotelId.HasValue)
            {
                query = query.Where(r => r.Room != null && r.Room.HotelId == hotelId.Value);
            }

            if (!string.IsNullOrEmpty(guestId))
            {
                query = query.Where(r => r.GuestId == guestId);
            }

            if (roomId.HasValue)
            {
                query = query.Where(r => r.RoomId == roomId.Value);
            }

            if (from.HasValue)
            {
                query = query.Where(r => r.CheckOutDate >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(r => r.CheckInDate <= to.Value);
            }

            if (active.HasValue)
            {
                query = query.Where(r => r.IsAvailable == active.Value);
            }

            // Execute the query
            var reservations = await query.ToListAsync();

            // Debug output
            Console.WriteLine($"Found {reservations.Count} reservations matching criteria");
            if (reservations.Any())
            {
                Console.WriteLine($"First reservation: ID {reservations[0].Id}, Guest: {reservations[0].Guest?.FirstName}");
            }

            return reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                CheckInDate = r.CheckInDate,
                CheckOutDate = r.CheckOutDate,
                GuestName = r.Guest != null ? $"{r.Guest.FirstName} {r.Guest.LastName}" : "Unknown Guest",
                GuestId = r.GuestId,
                RoomId = r.RoomId,
                RoomName = r.Room?.Name ?? "Unknown Room",
                HotelId = r.Room?.HotelId,
                HotelName = r.Room?.Hotel?.Name ?? "Unknown Hotel",
                IsActive = r.IsAvailable
            }).ToList();
        }
        public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Guest)
                .Include(r => r.Room)
                    .ThenInclude(room => room.Hotel)
                .ToListAsync();

            return reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                CheckInDate = r.CheckInDate,
                CheckOutDate = r.CheckOutDate,
                GuestName = r.Guest != null ? $"{r.Guest.FirstName} {r.Guest.LastName}" : "Unknown Guest",
                GuestId = r.GuestId,
                RoomId = r.RoomId,
                RoomName = r.Room?.Name ?? "Unknown Room",
                HotelId = r.Room?.HotelId,
                HotelName = r.Room?.Hotel?.Name ?? "Unknown Hotel",
                IsActive = r.IsAvailable
            }).ToList();
        }


    }
}
