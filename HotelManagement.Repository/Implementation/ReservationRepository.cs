using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Entities;
using HotelManagement.Repository.Abstraction;
using HotelManagement.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Repository.Implementation
{
    public class ReservationRepository:BaseRepository<Reservation>,IReservationRepository
    {
        private readonly ApplicationDbContext _context;
        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Reservation> GetByIdAsync(int id) => await _context.Reservations.FindAsync(id);

        public async Task<bool> IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut)
        {
            return !await _context.Reservations.AnyAsync(r =>
                r.RoomId == roomId &&
                ((checkIn >= r.CheckInDate && checkIn < r.CheckOutDate) ||
                 (checkOut > r.CheckInDate && checkOut <= r.CheckOutDate) ||
                 (checkIn <= r.CheckInDate && checkOut >= r.CheckOutDate))
            );
        }

        public async Task<IEnumerable<Reservation>> SearchAsync(int? hotelId, int? guestId, int? roomId, DateTime? from, DateTime? to, bool? active)
        {
            var query = _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .AsQueryable();

            if (hotelId.HasValue)
                query = query.Where(r => r.Room.HotelId == hotelId);

            if (guestId.HasValue)
                query = query.Where(r => r.GuestId == guestId);

            if (roomId.HasValue)
                query = query.Where(r => r.RoomId == roomId);

            if (from.HasValue)
                query = query.Where(r => r.CheckInDate >= from);

            if (to.HasValue)
                query = query.Where(r => r.CheckOutDate <= to);

            if (active.HasValue)
            {
                var today = DateTime.Today;
                query = active.Value
                    ? query.Where(r => r.CheckOutDate >= today)
                    : query.Where(r => r.CheckOutDate < today);
            }

            return await query.ToListAsync();
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

    }
}
