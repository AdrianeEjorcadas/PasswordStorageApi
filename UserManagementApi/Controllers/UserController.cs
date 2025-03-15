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
        public async Task<ActionResult<UserCredentialModel>> CreateUser([FromBody] AddUserDTO addUserDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var user = await _userService.CreateUserAsync(addUserDTO);
                return Ok(user);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { Message = aex.Message });
            }
            catch (Exception ex) 
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("change-password")]
        public async Task<ActionResult<UserCredentialModel>> ChangePasswordAsync([FromBody] string userId, [FromBody] ChangePasswordDTO changePasswordDTO) 
        {
            try
            {
                if (!ModelState.IsValid) 
                    return BadRequest(ModelState);
                await _userService.ChangePasswordAsync(userId, changePasswordDTO);
                return Ok("Your password has been successfully updated.");
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { Message = aex.Message });
            }
            catch (Exception ex) 
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

    }
}
