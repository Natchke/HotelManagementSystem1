using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Entities;

namespace HotelManagement.Repository.Abstraction
{
   public interface IReservationRepository:IBaseRepository<Reservation>
    {
        Task<bool> IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut);
        Task<Reservation> GetByIdAsync(int id);
        Task<IEnumerable<Reservation>> SearchAsync(int? hotelId, int? guestId, int? roomId, DateTime? from, DateTime? to, bool? active);
        Task SaveAsync();
    }
}
