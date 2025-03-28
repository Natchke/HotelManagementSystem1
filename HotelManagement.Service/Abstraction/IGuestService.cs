using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Guest;

namespace HotelManagement.Service.Abstraction
{
    public interface IGuestService
    {
        Task RegisterAsync(GuestRegistrationDto dto);
        Task UpdateAsync(GuesUpdatingDto dto);
        Task<IEnumerable<GuestDto>> GetAllAsync();
        Task<bool> DeleteAsync(string id);
    }
}
