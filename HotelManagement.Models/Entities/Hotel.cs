using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Models.Entities
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        // One-to-One with Manager
        public int ManagerId { get; set; }
        public Manager Manager { get; set; }

        // One-to-Many with Room
        public List<Room> Rooms { get; set; }

        // Many-to-Many with Reservation
        public List<Reservation> Reservations { get; set; }

    }
}
