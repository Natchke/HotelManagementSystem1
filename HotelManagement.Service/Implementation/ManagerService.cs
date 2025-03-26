using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Manager;
using HotelManagement.Models.Entities;
using HotelManagement.Repository.Abstraction;
using HotelManagement.Service.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HotelManagement.Service.Implementation
{
    public  class ManagerService : IManagerService
    {
        private readonly IManagerRepository _repo;

        public ManagerService(IManagerRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> RegisterAsync(ManagerRegistrationDto dto)
        {
            if (await _repo.GetByEmailAsync(dto.Email) != null)
                throw new Exception("Email is already registered.");

            if (await _repo.GetByPersonalNumberAsync(dto.PersonalNumber) != null)
                throw new Exception("Personal Number must be unique.");

            var manager = new Manager
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PersonalNumber = dto.PersonalNumber,
                password = dto.password,
                MobileNumber = dto.MobileNumber,
                HotelId = dto.HotelId
            };

            await _repo.AddAsync(manager);
            await _repo.SaveAsync();

            return "Manager successfully registered.";
        }

        public async Task<string> LoginAsync(MangerLoginDto dto)
        {
            var manager = await _repo.GetByEmailAsync(dto.Email);
            if (manager == null || manager.password != dto.Password)
                throw new Exception("Invalid credentials.");

            return "Manager logged in.";
        }

        public async Task UpdateAsync(ManagerUpdatingDto dto)
        {
            var manager = await _repo.GetByIdAsync(dto.Id);
            if (manager == null)
                throw new Exception("Manager not found.");

            manager.FirstName = dto.FirstName;
            manager.LastName = dto.LastName;
            manager.Email = dto.Email;
            manager.MobileNumber = dto.MobileNumber;

            await _repo.UpdatePartialAsync(manager);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var manager = await _repo.GetByIdAsync(id);
            if (manager == null) return false;

            var hotelManagers = await _repo.GetManagersByHotelAsync(manager.HotelId);
            if (hotelManagers.Count() <= 1)
                throw new Exception("Cannot delete the only manager of the hotel.");

            _repo.Remove(manager);
            await _repo.SaveAsync();
            return true;
        }
    }
}
