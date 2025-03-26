using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Models.Dtos.Hotel
{
    public class HotelForUpdatingDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
