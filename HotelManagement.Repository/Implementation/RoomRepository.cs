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
    public  class RoomRepository:BaseRepository<Room>,IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddAsync(Room room)
        {
            if (room.Price <= 0)
                throw new ArgumentException("Room price must be greater than zero.");

            await base.AddAsync(room);
            await base.Save();
        }

        public async Task UpdateAsync(Room room)
        {
            var roomFromDb = await _context.Rooms.FindAsync(room.Id);

            if (roomFromDb != null)
            {
                if (room.Price <= 0)
                    throw new ArgumentException("Room price must be greater than zero.");

                roomFromDb.Name = room.Name;
                roomFromDb.Price = room.Price;
                roomFromDb.IsAvailable = room.IsAvailable;

                _context.Rooms.Update(roomFromDb);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(int roomId)
        {
            var room = await _context.Rooms
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null) return false;

            bool hasActiveBookings = room.Reservations?.Any(r => r.IsAvailable) ?? false;

            if (hasActiveBookings) return false;

            Remove(room);
            await Save();
            return true;
        }

        public async Task<Room> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.Reservations) 
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Room>> FilterRoomsAsync(int? hotelId, bool? isAvailable, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Rooms.AsQueryable();

            if (hotelId.HasValue)
                query = query.Where(r => r.HotelId == hotelId);

            if (isAvailable.HasValue)
                query = query.Where(r => r.IsAvailable == isAvailable);

            if (minPrice.HasValue)
                query = query.Where(r => r.Price >= minPrice);

            if (maxPrice.HasValue)
                query = query.Where(r => r.Price <= maxPrice);

            return await query.ToListAsync();
        }
    }
}
