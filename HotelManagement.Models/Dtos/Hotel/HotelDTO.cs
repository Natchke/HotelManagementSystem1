using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Managerr;
using HotelManagement.Models.Dtos.Reservations;
using HotelManagement.Models.Dtos.Room;
using HotelManagement.Models.Entities;

namespace HotelManagement.Models.Dtos.Hotel
{
    public class HotelDTO
    {
         
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int ManagerId { get; set; }

        public ManagerDto Manager { get; set; }

        public List<RoomDto> Rooms { get; set; } = new List<RoomDto>(); 
        public List<ReservationDto> Reservations { get; set; } = new List<ReservationDto>();


    }
}
