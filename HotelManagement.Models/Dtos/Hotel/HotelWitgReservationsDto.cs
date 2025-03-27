using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Managerr;
using HotelManagement.Models.Dtos.Room;

namespace HotelManagement.Models.Dtos.Hotel
{
    public  class HotelWitgReservationsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public ManagerDto Manager { get; set; }
        public List<RoomWithReservationDto> Rooms { get; set; } = new List<RoomWithReservationDto>();
    }
}
