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
    public class ManagerRepository : BaseRepository<Manager>, IManagerRepository
    {
        private readonly ApplicationDbContext _context;
        public ManagerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Manager> GetByEmailAsync(string email) =>
            await _context.Managers.FirstOrDefaultAsync(m => m.Email == email);

        public async Task<Manager> GetByPersonalNumberAsync(string personalNumber) =>
            await _context.Managers.FirstOrDefaultAsync(m => m.PersonalNumber == personalNumber);

        public async Task<Manager> GetByIdAsync(string id) =>
            await _context.Managers.FindAsync(id);

        public async Task<IEnumerable<Manager>> GetManagersByHotelAsync(int hotelId) =>
            await _context.Managers.Where(m => m.HotelId == hotelId).ToListAsync();


        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public async Task UpdatePartialAsync(Manager manager)
        {
            var existing = await _context.Managers.FindAsync(manager.Id);
            if (existing != null)
            {
                existing.FirstName = manager.FirstName;
                existing.LastName = manager.LastName;
                existing.Email = manager.Email;
                existing.PhoneNumber = manager.PhoneNumber;
                await _context.SaveChangesAsync();
            }
        }
    }
}
