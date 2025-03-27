using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Reservations;

namespace HotelManagement.Models.Dtos.Room
{
    public  class RoomWithReservationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public List<ReservationShortDto> Reservations { get; set; } = new List<ReservationShortDto>();
    }
}
