using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Hotel;
using HotelManagement.Models.Dtos.Reservations;

namespace HotelManagement.Models.Dtos.Room
{
    public  class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public int HotelId { get; set; }

        public string GuestName { get; set; }
        public HotelShortInfoDto Hotel { get; set; }
        public List<ReservationDto> Reservations { get; set; } = new List<ReservationDto>();

    }
}
