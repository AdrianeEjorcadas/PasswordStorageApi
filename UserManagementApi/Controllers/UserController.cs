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
                return BadRequest(new { ErrorMessage = aex.Message });
            }
            catch (Exception ex) 
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordDTO changePasswordDTO) 
        {
            try
            {
                if (!ModelState.IsValid) 
                    return BadRequest(ModelState);
                await _userService.ChangePasswordAsync(changePasswordDTO);
                return Ok("Your password has been successfully updated.");
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { ErrorMessage = aex.Message });
            }
            catch (Exception ex) 
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                await _userService.ForgotPasswordAsync(forgotPasswordDTO.EmailAddress);
                return Ok("If the email exists, a password reset link has been sent.");
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { ErrorMessage = aex.Message });
            }
            catch (Exception ex) 
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPasswordAsync(
                        [FromQuery] string token, 
                        [FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var userCreds = await _userService.ResetPasswordAsync(token, resetPasswordDTO);
                return Ok("Your password has been successfully updated.");
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new {ErrorMessage=aex.Message});
            } catch(Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("login")]
        public async Task<ActionResult> LoginAsync(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }

    }
}

