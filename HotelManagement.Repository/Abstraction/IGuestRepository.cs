using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Entities;

namespace HotelManagement.Repository.Abstraction
{
    public  interface IGuestRepository:IBaseRepository<Guest>
    {
        Task<Guest> GetByPersonalNumberAsync(string personalNumber);
        Task<Guest> GetByMobileAsync(string mobile);
        Task<IEnumerable<Guest>> GetAllAsync();
        Task<Guest> GetByIdAsync(string id);
        Task<bool> HasActiveReservationAsync(string guestId);
        Task SaveAsync();
    }
}
