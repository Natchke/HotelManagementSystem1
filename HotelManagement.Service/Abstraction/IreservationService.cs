using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Reservations;
using HotelManagement.Models.Entities;

namespace HotelManagement.Service.Abstraction
{
    public  interface IreservationService
    {
        Task CreateAsync(ReservationCreateDto dto);
        Task UpdateAsync(ReservationUpdateDto dto);
        Task CancelAsync(int id);
        Task<IEnumerable<Reservation>> SearchAsync(int? hotelId, int? guestId, int? roomId, DateTime? from, DateTime? to, bool? active);
    }
}
