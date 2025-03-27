using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Models.Dtos.Reservations
{
    public  class ReservationDto
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string GuestName { get; set; }
        public string GuestId { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string HotelName { get; set; }
        public bool IsActive { get; set; }
        public int? HotelId { get; set; }
    }
}
