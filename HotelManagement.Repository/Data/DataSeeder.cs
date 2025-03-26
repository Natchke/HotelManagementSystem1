using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Repository.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();


            await SeedRoles();

            await SeedUsers();

            await SeedHotels();
        }

        private async Task SeedRoles()
        {
            string[] roles = { "Admin", "Manager", "Guest" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task SeedUsers()
        {
            if (await _userManager.FindByEmailAsync("admin@example.com") == null)
            {
                var admin = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }

        private async Task SeedHotels()
        {
            if (!_context.Hotels.Any())
            {
                var hotel1 = new Hotel { Id = 1, Name = "Grand Hotel", Rating = 5, Country = "USA", City = "New York", Address = "123 Main St" };
                var hotel2 = new Hotel { Id = 2, Name = "Sea View Resort", Rating = 4, Country = "Greece", City = "Santorini", Address = "456 Ocean Rd" };

                var manager1 = new Manager { Id = 1, FirstName = "davit", LastName = "afxazishvili",  Email = "manager1@example.com",PersonalNumber="12345678901", MobileNumber = "123456789", Hotel = hotel1 };
                var manager2 = new Manager { Id = 2, FirstName = "Anna", LastName = "kharebava",  Email = "manager2@example.com",PersonalNumber="11111111111", MobileNumber = "987654321", Hotel = hotel2 };

                var room1 = new Room { Id = 1, Name = "Deluxe Suite", IsAvailable = true, Price = 200, HotelId = 1 };
                var room2 = new Room { Id = 2, Name = "Standard Room", IsAvailable = true, Price = 100, HotelId = 1 };
                var room3 = new Room { Id = 3, Name = "Ocean View Room", IsAvailable = false, Price = 250, HotelId = 2 };

                await _context.Hotels.AddRangeAsync(hotel1, hotel2);
                await _context.Managers.AddRangeAsync(manager1, manager2);
                await _context.Rooms.AddRangeAsync(room1, room2, room3);

                await _context.SaveChangesAsync();
            }
        }
    }
}
