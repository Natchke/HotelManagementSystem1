using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Hotel;
using HotelManagement.Models.Entities;
using HotelManagement.Repository.Abstraction;
using HotelManagement.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Repository.Implementation
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        private readonly ApplicationDbContext _context;

        public HotelRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        
        public async Task AddAsync(Hotel hotel)
        {
            await base.AddAsync(hotel); // from BaseRepository
            await base.Save();          // from BaseRepository
        }

       
        public async Task UpdateAsync(Hotel hotel)
        {
            var hotelFromDb = await _context.Hotels.FindAsync(hotel.Id);

            if (hotelFromDb != null)
            {
                if (hotel.Rating < 1 || hotel.Rating > 5)
                    throw new ArgumentException("Rating must be between 1 and 5.");

                hotelFromDb.Name = hotel.Name;
                hotelFromDb.Address = hotel.Address;
                hotelFromDb.Rating = hotel.Rating;

                _context.Hotels.Update(hotelFromDb);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _context.Hotels.ToListAsync();
        }


        public async Task<Hotel> GetByIdAsync(int id)
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .Include(h => h.Manager)
                .Include(h => h.Reservations)
                .FirstOrDefaultAsync(h => h.Id == id);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Rooms)
                .Include(h => h.Reservations)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null)
                return false;

            bool hasActiveRooms = hotel.Rooms?.Any(r => r.IsAvailable) ?? false;
            bool hasActiveReservations = hotel.Reservations?.Any(r => r.IsAvailable) ?? false;

            if (hasActiveRooms || hasActiveReservations)
                return false;

            base.Remove(hotel); 
            await base.Save();  
            return true;
        }


        public async Task<IEnumerable<Hotel>> GetFilteredHotelsAsync(string country, string city, int? rating, string includeProperties = null)
        {
            var query = _context.Hotels.AsQueryable();

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            if (!string.IsNullOrWhiteSpace(country))
                query = query.Where(h => h.Country == country);

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(h => h.City == city);

            if (rating.HasValue)
                query = query.Where(h => h.Rating == rating);

            return await query.ToListAsync();
        }

    }
}

