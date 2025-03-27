using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Models.Dtos.Guest
{
    public class GuestForGettingDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        //public IEnumerable<ReservationForGuestsDto> Reservations { get; set; }
    }
}
