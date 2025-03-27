using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Entities;

namespace HotelManagement.Repository.Abstraction
{
    public  interface IManagerRepository : IBaseRepository<Manager>
    {
        Task<Manager> GetByEmailAsync(string email);
        Task<Manager> GetByPersonalNumberAsync(string personalNumber);
        Task<Manager> GetByIdAsync(string id);
        Task<IEnumerable<Manager>> GetManagersByHotelAsync(int hotelId);
        Task UpdatePartialAsync(Manager manager);
        Task SaveAsync();
    }
}

