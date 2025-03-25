using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UserManagementApi.CustomExceptions;
using UserManagementApi.DTO;
using UserManagementApi.Messages;
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
                return StatusCode(500, new {ErrorMessage = ErrorMessages.ExceptionDefault, Details = ex.Message});
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
            catch (InvalidCredentialsException icx)
            {
                return BadRequest(new { ErrorMessage = icx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = ErrorMessages.ExceptionDefault, Details = ex.Message });
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
            catch (InvalidCredentialsException iex)
            {
                return BadRequest(new { ErrorMessage = iex.Message });
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { ErrorMessage = aex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = ErrorMessages.ExceptionDefault, Details = ex.Message });
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
            catch (InvalidCredentialsException icx)
            {
                return StatusCode(401, new {ErrorMessage=icx.Message});
            } 
            catch (InvalidTokenException tix)
            {
                return StatusCode(401, new { ErrorMessage = tix.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = ErrorMessages.ExceptionDefault, Details = ex.Message });
            }
        }

        [HttpPut("login")]
        public async Task<ActionResult> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var token = await _userService.LoginAsync(loginDTO);
                return Ok(token);
            }
            catch (InvalidCredentialsException icx)
            {
                return StatusCode(401, new { ErrorMessage = icx.Message });
            } 
            catch(AccountLockedException alx)
            {
                return StatusCode(423, new {ErrorMessage = alx.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = ErrorMessages.ExceptionDefault, Details = ex.Message });
            }
        }

        [HttpGet("validate-token")]
        public async Task<ActionResult> ValidateTokenAsync()
        {
            try
            {
                var authTokenWithBearer = Request.Headers["Authorization"].ToString();
                var authToken = authTokenWithBearer.Replace("Bearer ", "").Trim();
                await _userService.ValidateTokenAsync(authToken);
                return Ok();
            }
            catch (InvalidTokenException tx)
            {
                return StatusCode(401, new { ErrorMessage = tx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = ErrorMessages.ExceptionDefault, Details = ex.Message });
            }
        }

    }
}

