using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Delete;
using HotelManagement.Models.Dtos.Hotel;
using HotelManagement.Models.Entities;

namespace HotelManagement.Service.Abstraction
{
    public interface IHotelService
    {
        Task AddHotelAsync(HotelForCreatingDto dto);
        Task UpdateHotelAsync(HotelForUpdatingDto dto);
        Task<DeleteResultDto> DeleteHotelAsync(int id);
        Task<HotelWitgReservationsDto> GetHotelByIdAsync(int id);
       
    
    Task<IEnumerable<HotelWitgReservationsDto>> GetFilteredHotelsAsync(string country, string city, int? rating);
    }
}
