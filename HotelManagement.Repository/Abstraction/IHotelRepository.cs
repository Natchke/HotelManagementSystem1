using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Hotel;
using HotelManagement.Models.Entities;

namespace HotelManagement.Repository.Abstraction
{
    public interface IHotelRepository : IBaseRepository<Hotel>
    {
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<Hotel> GetByIdAsync(int id);
        Task AddAsync(Hotel hotel);
        Task UpdateAsync(Hotel hotel);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Hotel>> GetFilteredHotelsAsync(string country, string city, int? rating,string  includeProperties = null);
    }
}
