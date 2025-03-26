using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Manager;

namespace HotelManagement.Service.Abstraction
{
    public interface IManagerService
    {
        Task<string> RegisterAsync(ManagerRegistrationDto dto);
        Task<string> LoginAsync(MangerLoginDto dto);
        Task UpdateAsync(ManagerUpdatingDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
