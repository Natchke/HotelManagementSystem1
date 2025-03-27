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
        
        Task UpdateAsync(ManagerUpdatingDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
