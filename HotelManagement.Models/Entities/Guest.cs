using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HotelManagement.Models.Entities
{
    public class Guest : ApplicationUser
    {
        

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        
        [Required,MaxLength(11)]
        public string PersonalNumber { get; set; }


        
        public ICollection<Reservation> Reservations { get; set; }
      
    }
}
