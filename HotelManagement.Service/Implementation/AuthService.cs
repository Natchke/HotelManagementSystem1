using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelManagement.Models.Dtos.Identity;
using HotelManagement.Models.Dtos.Manager;
using HotelManagement.Models.Entities;
using HotelManagement.Repository.Abstraction;
using HotelManagement.Service.Abstraction;
using HotelManagement.Service.Exeptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Service.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;

        private const string AdminRole = "Admin";
        private const string UserRole = "User";
        private const string ManagerRole = "Manager";

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper,
            IHotelRepository hotelRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _hotelRepository = hotelRepository;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByNameAsync(loginRequestDto.UserName);
            if (user == null)
                throw new Exception($"User {loginRequestDto.UserName} not found");

            var isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (!isValid)
                return new LoginResponseDto { Token = string.Empty };

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            return new LoginResponseDto { Token = token };
        }

        public async Task RegisterUser(RegistrationRequestDto registrationRequestDto)
        {
            await EnsureRoleExists(UserRole);

            var existingUser = await _userManager.Users
                .Where(u => u is Guest)
                .Cast<Guest>()
                .FirstOrDefaultAsync(u => u.PersonalNumber == registrationRequestDto.PersonalNumber);

            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this Personal Number already exists.");
            }

            var user = new Guest
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                PhoneNumber = registrationRequestDto.PhoneNumber,
                FirstName = registrationRequestDto.FirstName,
                LastName = registrationRequestDto.LastName,
                PersonalNumber = registrationRequestDto.PersonalNumber
            };

            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(result.Errors.FirstOrDefault()?.Description);

            await _userManager.AddToRoleAsync(user, UserRole);
        }

        public async Task RegisterManager(ManagerRegistrationDto registrationRequestDto)
        {
            await EnsureRoleExists(ManagerRole);

            var existingManager = await _userManager.Users
                .Where(u => u is Manager)
                .Cast<Manager>()
                .FirstOrDefaultAsync(u => u.PersonalNumber == registrationRequestDto.PersonalNumber);

            if (existingManager != null)
            {
                throw new InvalidOperationException("A manager with this Personal Number already exists.");
            }

            var user = new Manager
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                PhoneNumber = registrationRequestDto.MobileNumber,
                FirstName = registrationRequestDto.FirstName,
                LastName = registrationRequestDto.LastName,
                PersonalNumber = registrationRequestDto.PersonalNumber,
                HotelId = registrationRequestDto.HotelId
            };

            var result = await _userManager.CreateAsync(user, registrationRequestDto.password);
            if (!result.Succeeded)
                throw new InvalidOperationException(result.Errors.FirstOrDefault()?.Description);

            await _userManager.AddToRoleAsync(user, ManagerRole);

            // Update the Hotel's ManagerId after successful manager registration
            if (registrationRequestDto.HotelId != null)
            {
                var hotel = await _hotelRepository.GetAsync(h => h.Id == registrationRequestDto.HotelId);
                if (hotel != null)
                {
                    hotel.ManagerId = user.Id;
                    await _hotelRepository.SaveChangesAsync();
                }
            }
        }

        public async Task RegisterAdmin(AdminRegistrationDto registrationRequestDto)
        {
            await EnsureRoleExists(AdminRole);

            var user = new ApplicationUser
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(result.Errors.FirstOrDefault()?.Description);

            await _userManager.AddToRoleAsync(user, AdminRole);
        }

        private async Task EnsureRoleExists(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
