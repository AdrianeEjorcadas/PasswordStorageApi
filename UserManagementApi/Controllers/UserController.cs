using Microsoft.AspNetCore.Mvc;
using UserManagementApi.DTO;
using UserManagementApi.Models;
using UserManagementApi.Services;

namespace UserManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create-user")]
        public async Task<ActionResult<UserModel>> CreateUser([FromBody] AddUserDTO addUserDTO)
        {
            try
            {
                var user = _userService.CreateUserAsync(addUserDTO);
                return Ok(addUserDTO);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { aex.Message });
            }
            catch (Exception ex) 
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
