using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UserManagementApi.CustomExceptions;
using UserManagementApi.DTO;
using UserManagementApi.Filters;
using UserManagementApi.Messages;
using UserManagementApi.Models;
using UserManagementApi.Repositories;
using UserManagementApi.Services;

namespace UserManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISessionRepository _sessionRepository;
        public UserController(IUserService userService, ISessionRepository sessionRepository)
        {
            _userService = userService;
            _sessionRepository = sessionRepository;
        }

        [HttpPost("create-user")]
        [ValidateModelState]
        public async Task<ActionResult<UserCredentialModel>> CreateUser([FromBody] AddUserDTO addUserDTO)
        {
            try
            {
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

        [HttpPut("confirm-email")]
        [ValidateModelState]
        public async Task<ActionResult> ConfirmEmailAsync([FromQuery] ConfirmationEmailDTO confirmationDTO)
        {
            try
            {
                await _userService.ValidateEmailTokenAsync(confirmationDTO);    
                return Ok();
            }
            catch (RevokedTokenException ex) 
            {
                return BadRequest(new { ErrorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = ErrorMessages.ExceptionDefault, Details = ex.Message });
            }
        }

        [HttpPut("change-password")]
        [ValidateModelState]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordDTO changePasswordDTO) 
        {
            try
            {
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
        [ValidateModelState]
        public async Task<ActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            try
            {
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
        [ValidateModelState]
        public async Task<ActionResult> ResetPasswordAsync(
                        [FromQuery] string token, 
                        [FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
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
        [ValidateModelState]
        public async Task<ActionResult> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var token = await _userService.LoginAsync(loginDTO);
                return Ok(token);
            }
            catch (InvalidCredentialsException ex)
            {
                return StatusCode(401, new ErrorResponse
                {
                    StatusCode = 401,
                    ErrorMessage = ex.Message
                });
            } 
            catch(AccountLockedException ex)
            {
                return StatusCode(423, new ErrorResponse
                {
                    StatusCode = 423,
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    StatusCode = 500,
                    ErrorMessage = ErrorMessages.ExceptionDefault,
                    Details = ex.Message
                });
            }
        }

        //[HttpGet("validate-token")]
        //public async Task<ActionResult> ValidateTokenAsync()
        //{
        //    try
        //    {
        //        if (!Request.Headers.ContainsKey("Authorization"))
        //        {
        //            return BadRequest(new { ErrorMessage = "Authorization header is missing."});
        //        }
        //        var authTokenWithBearer = Request.Headers["Authorization"].ToString();
        //        var authToken = authTokenWithBearer.Replace("Bearer ", "").Trim();
        //        await _userService.ValidateTokenAsync(authToken);
        //        return Ok();
        //    }
        //    catch(RevokedTokenException rtx) 
        //    {
        //        return StatusCode(403, new { ErrorMessage =  rtx.Message }); // status 403 will end the user session
        //    }
        //    catch (InvalidTokenException tx)
        //    {
        //        return StatusCode(401, new { ErrorMessage = tx.Message }); // status 401 will be the trigger to generate new token to continue the session
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { ErrorMessage = ErrorMessages.ExceptionDefault, Details = ex.Message });
        //    }
        //}

        [HttpPut("generate-new-token")]
        [ServiceFilter(typeof(ValidateTokenFilter))]
        public async Task<ActionResult> GenerateNewTokenAsync([FromBody] string refreshToken)
        {
            try
            {
                #region remove header process
                //if (!Request.Headers.ContainsKey("Refresh-Token"))
                //{
                //    return BadRequest(new { ErrorMessage = "Authorization header is missing." });
                //}
                //var refTokenWithBearer = Request.Headers["Refresh-Token"].ToString();
                //var refToken = refTokenWithBearer.Replace("Bearer ", "").Trim(); 
                #endregion
                await _userService.GenerateNewTokenAsync(refreshToken);
                return Ok();
            }
            catch(InvalidOperationException ex)
            {
                return StatusCode(400, new { ErrorMessages = ErrorMessages.ExceptionDefault, Details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessages = ErrorMessages.ExceptionDefault, Details = ex.Message });
            }
        }

        [HttpPut("logout")]
        [ServiceFilter(typeof(ValidateTokenFilter))]
        public async Task<ActionResult> LogoutAsync([FromBody] AuthenticationTokenDetailsDTO tokenDetailsDTO)
        {
            try
            {
                await _userService.LogoutAsync(tokenDetailsDTO);
                await _sessionRepository.ClearHeaderAsync();
                return Ok();
            } 
            catch (InvalidTokenException ex)
            {
                return StatusCode(403, new ErrorResponse
                {
                    StatusCode = 403,
                    ErrorMessage = ex.Message
                });
            }
            catch (InvalidCredentialsException ex)
            {
                return StatusCode(401, new ErrorResponse
                {
                    StatusCode = 401,
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    StatusCode = 500,
                    ErrorMessage = ErrorMessages.ExceptionDefault,
                    Details = ex.Message
                });
            }
        }

    }
}

