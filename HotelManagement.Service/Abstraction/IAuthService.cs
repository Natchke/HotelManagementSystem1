using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Dtos.Identity;
using HotelManagement.Models.Dtos.Manager;

namespace HotelManagement.Service.Abstraction
{
    public  interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task RegisterUser(RegistrationRequestDto registrationRequestDto);
        Task RegisterManager(ManagerRegistrationDto registrationRequestDto);
        Task RegisterAdmin(AdminRegistrationDto registrationRequestDto);
    }
}
