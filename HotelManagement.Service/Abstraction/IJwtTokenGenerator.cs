using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace HotelManagement.Service.Abstraction
{
    public interface  IJwtTokenGenerator
    {
        string GenerateToken(IdentityUser User, IEnumerable<string> roles);
    }
}
