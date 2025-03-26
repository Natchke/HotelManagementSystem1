using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Hotel;
using HotelManagement.Models.Entities;

namespace HotelManagement.Service.Abstraction
{
    public interface IHotelService
    {
        Task AddHotelAsync(HotelForCreatingDto dto);
        Task UpdateHotelAsync(HotelForUpdatingDto dto);
        Task<bool> DeleteHotelAsync(int id);
        Task<HotelDTO> GetHotelByIdAsync(int id);
        Task<IEnumerable<HotelDTO>> GetFilteredHotelsAsync(string country, string city, int? rating);
    }
}
