using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Guest;

namespace HotelManagement.Models.Dtos.Reservations
{
    public class ReservationShortDto
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string GuestName { get; set; }
        public string? GuestContact { get; set; }
        public string Status { get; set; }
    }
}
