using HotelManagement.Models.Dtos.Manager;
using HotelManagement.Service.Abstraction;
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
