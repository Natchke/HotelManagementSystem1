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
    public class GuestRepository:BaseRepository<Guest>,IGuestRepository
    {
        private readonly ApplicationDbContext _context;
        public GuestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Guest> GetByPersonalNumberAsync(string personalNumber) =>
            await _context.Guests.FirstOrDefaultAsync(g => g.PersonalNumber == personalNumber);

        public async Task<Guest> GetByMobileAsync(string mobile) =>
            await _context.Guests.FirstOrDefaultAsync(g => g.MobileNumber == mobile);

        public async Task<Guest> GetByIdAsync(int id) =>
            await _context.Guests.FindAsync(id);

        public async Task<bool> HasActiveReservationAsync(int guestId) =>
            await _context.Reservations.AnyAsync(r => r.Id == guestId && r.CheckOutDate >= DateTime.Today);

        public async Task SaveAsync() => await _context.SaveChangesAsync();

    }
}
