using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Models.Entities
{
    public class Manager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        [Required,MaxLength(11)]
        public string PersonalNumber {  get; set; }
        [Required]
        public string password { get; set; }    

        [Required]
        public string MobileNumber { get; set; }

        // One-to-One with Hotel
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
