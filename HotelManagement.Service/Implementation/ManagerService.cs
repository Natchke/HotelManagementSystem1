using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Manager;
using HotelManagement.Models.Dtos.Managerr;
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

        public async Task<IEnumerable<ManagerDto>> GetAllManagersAsync()
        {
            var managers = await _repo.GetAllAsync();
            return managers.Select(m => new ManagerDto
            {
                Id = m.Id,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email,
                MobileNumber = m.PhoneNumber,
                
            }).ToList();
        }

        public async Task<ManagerDto?> GetManagerByIdAsync(string id)
        {
            var manager = await _repo.GetByIdAsync(id);
            if (manager == null) return null;

            return new ManagerDto
            {
                Id = manager.Id,
                FirstName = manager.FirstName,
                LastName = manager.LastName,
                Email = manager.Email,
                MobileNumber = manager.PhoneNumber,
                
            };
        }


        public async Task UpdateAsync(ManagerUpdatingDto dto)
        {
            var manager = await _repo.GetByIdAsync(dto.Id);
            if (manager == null)
                throw new Exception("Manager not found.");

            manager.FirstName = dto.FirstName;
            manager.LastName = dto.LastName;
            manager.Email = dto.Email;
            manager.PhoneNumber = dto.MobileNumber;

            await _repo.UpdatePartialAsync(manager);
        }

        public async Task<bool> DeleteAsync(string id) //aq ragac iseveriko pirobashi xoda gadavwkviote ro sasturos cotaxnit umenejerod davtoveb da rorame vakansiasgamovacxadeb mere 
        {
            var manager = await _repo.GetByIdAsync(id);
            if (manager == null) return false;

            var hotelManagers = await _repo.GetManagersByHotelAsync(manager.HotelId);
            

            _repo.Remove(manager);
            await _repo.SaveAsync();
            return true;
        }
    }
}
