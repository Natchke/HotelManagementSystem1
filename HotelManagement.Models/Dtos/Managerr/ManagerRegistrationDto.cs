using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Models.Dtos.Manager
{
    public  class ManagerRegistrationDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PersonalNumber { get; set; }
        [Required]
        public string password { get; set; }

        [Required]
        public string MobileNumber { get; set; }
        public int HotelId { get; set; }
    }
}
