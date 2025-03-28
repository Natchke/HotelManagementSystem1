using HotelManagement.Models.Dtos.Manager;
using HotelManagement.Service.Abstraction;
using HotelManagement.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem1.Controllers
{
    [ApiController]
    [Route("api/managers")]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _service;

        public ManagerController(IManagerService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllManagers()
        {
            var managers = await _service.GetAllManagersAsync();
            return Ok(new ApiResponse("Managers retrieved successfully", managers, 200, true));
        }

        [HttpGet("{id}")]
        [Authorize(Roles ="Admin,Manager")]
        public async Task<IActionResult> GetManagerById(string id)
        {
            var manager = await _service.GetManagerByIdAsync(id);
            if (manager == null)
                return NotFound(new ApiResponse("Manager not found", null, 404, false));

            return Ok(new ApiResponse("Manager retrieved successfully", manager, 200, true));
        }


        [HttpPut]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update([FromBody] ManagerUpdatingDto dto)
        {
            await _service.UpdateAsync(dto); 
            return Ok(new ApiResponse("Manager updated successfully.", null, 200, true));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _service.DeleteAsync(id);
            return success
                ? Ok(new ApiResponse("Manager deleted successfully.", null, 200, true))
                : NotFound(new ApiResponse("Manager not found.", null, 404, false));
        }
    }
}
