using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Guest;
using HotelManagement.Models.Entities;
using HotelManagement.Repository.Abstraction;
using HotelManagement.Service.Abstraction;

namespace HotelManagement.Service.Implementation
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _repo;
        public GuestService(IGuestRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<GuestDto>> GetAllAsync()
        {
            var guests = await _repo.GetAllAsync();

            return guests.Select(g => new GuestDto
            {
                Id = g.Id,
                FirstName = g.FirstName,
                LastName = g.LastName,
                Email = g.Email,
                MobileNumber = g.PhoneNumber,
                PersonalNumber = g.PersonalNumber
            }).ToList();
        }


        public async Task RegisterAsync(GuestRegistrationDto dto)
        {
            if (await _repo.GetByPersonalNumberAsync(dto.PersonalNumber) != null)
                throw new Exception("Personal Number already exists");
            if (await _repo.GetByMobileAsync(dto.MobileNumber) != null)
                throw new Exception("Mobile Number already exists");

            var guest = new Guest
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.MobileNumber,
                PersonalNumber = dto.PersonalNumber
            };

            await _repo.AddAsync(guest);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(GuesUpdatingDto dto)
        {
            var guest = await _repo.GetByIdAsync(dto.Id);
            if (guest == null) throw new Exception("Guest not found");

            guest.FirstName = dto.FirstName;
            guest.LastName = dto.LastName;
            guest.Email = dto.Email;
            guest.PhoneNumber = dto.MobileNumber;

            await _repo.SaveAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var guest = await _repo.GetByIdAsync(id);
            if (guest == null) return false;

            if (await _repo.HasActiveReservationAsync(id))
                throw new Exception("Guest has active reservations");

            _repo.Remove(guest);
            await _repo.SaveAsync();
            return true;
        }

       
    }
}
