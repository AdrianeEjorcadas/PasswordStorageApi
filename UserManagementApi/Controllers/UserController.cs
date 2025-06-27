using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
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

        [HttpGet("email-exist")]
        [ValidateModelState]
        public async Task<ActionResult> IsEmailExistAsync(string email) 
        {
            try
            {
                var isEmailExist = await _userService.IsEmailExistingAsync(email);
                return Ok(new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "Email confirmation successful",
                    Data = isEmailExist
                });
            }
            catch (Exception ex) 
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred",
                    Data = new { ExceptionMessage = ex.Message, StackTrace = ex.StackTrace }
                });
            }
        }

        [HttpPost("create-user")]
        [ValidateModelState]
        public async Task<ActionResult<ReturnResponse<UserCredentialModel>>> CreateUser([FromBody] AddUserDTO addUserDTO)
        {
            try
            {
                var user = await _userService.CreateUserAsync(addUserDTO);
                var response = new ReturnResponse<UserCredentialModel> 
                { 
                    StatusCode = 201,
                    Message = "User created successfully",
                    Data = null
                };
            
                return StatusCode(response.StatusCode, response);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new ReturnResponse<object>
                {
                    StatusCode = 400,
                    Message = aex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred",
                    Data = new { ExceptionMessage = ex.Message, StackTrace = ex.StackTrace }
                });
            }
        }

        [HttpPut("confirm-email")]
        [ValidateModelState]
        public async Task<ActionResult> ConfirmEmailAsync([FromQuery] ConfirmationEmailDTO confirmationDTO)
        {
            try
            {
                await _userService.ValidateEmailTokenAsync(confirmationDTO);

                return Ok( new ReturnResponse<string>
                {
                    StatusCode = 200,
                    Message = "Email confirmation successful",
                    Data = null
                });
            }
            catch (RevokedTokenException ex)
            {
                return BadRequest(new ReturnResponse<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred",
                    Data = new { ExceptionMessage = ex.Message, StackTrace = ex.StackTrace }
                });
            }
        }

        
        [HttpPut("resend-confirmation")]
        [ValidateModelState]
        public async Task<ActionResult> ResendConfirmationAsync([FromBody] ResendConfirmationDTO resendConfirmationDTO)
        {
            try
            {
                await _userService.ResendEmailTokenAsync(resendConfirmationDTO);
                return Ok(new ReturnResponse<string>
                {
                    StatusCode = 200,
                    Message = "Successfully resend the confirmation",
                    Data = null
                });
            } catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred",
                    Data = new { ExceptionMessage = ex.Message, StackTrace = ex.StackTrace }
                });
            }
        }

        [HttpPut("change-password")]
        [ValidateModelState]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordDTO changePasswordDTO) 
        {
            try
            {
                await _userService.ChangePasswordAsync(changePasswordDTO);
                return Ok(new ReturnResponse<string>
                {
                    StatusCode = 200,
                    Message = "Your password has been successfully updated.",
                    Data = null
                });
            }
            catch (InvalidCredentialsException ex)
            {
                return BadRequest(new ReturnResponse<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred",
                    Data = new { ExceptionMessage = ex.Message, StackTrace = ex.StackTrace }
                });
            }
        }

        [HttpPost("forgot-password")]
        [ValidateModelState]
        public async Task<ActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            try
            {
                await _userService.ForgotPasswordAsync(forgotPasswordDTO.EmailAddress);
                return Ok(new ReturnResponse<string>
                {
                    StatusCode = 200,
                    Message = "If the email exists, a password reset link has been sent.",
                    Data = null
                });
            }
            catch (InvalidCredentialsException ex)
            {
                return BadRequest(new ReturnResponse<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ReturnResponse<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred",
                    Data = new { ExceptionMessage = ex.Message, StackTrace = ex.StackTrace }
                });
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
                return Ok(new ReturnResponse<string>
                {
                    StatusCode = 200,
                    Message = "Your password has been successfully updated.",
                    Data = null
                });
            }
            catch (InvalidCredentialsException ex)
            {
                return StatusCode(401, new ReturnResponse<object>
                {
                    StatusCode = 401,
                    Message = ex.Message,
                    Data = ex.Data
                });
            } 
            catch (InvalidTokenException ex)
            {
                return StatusCode(401, new ReturnResponse<object>
                {
                    StatusCode = 401,
                    Message = ex.Message,
                    Data = ex.Data
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred",
                    Data = new { ExceptionMessage = ex.Message, StackTrace = ex.StackTrace }
                });
            }
        }

        [HttpPut("login")]
        [ValidateModelState]
        public async Task<ActionResult> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var token = await _userService.LoginAsync(loginDTO);
                return Ok(new ReturnResponse<object>
                {
                    StatusCode = (int)(HttpStatusCode.OK),
                    Message = "Login Successfully",
                    Data = new { token }
                });
            }
            catch (InvalidCredentialsException ex)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, new ReturnResponse<object>
                {
                    StatusCode = (int)(HttpStatusCode.Unauthorized),
                    Message = "Login Error",
                    Data = new { Error = ex.Message }
                });
            } 
            catch(AccountLockedException ex)
            {
                return StatusCode((int)HttpStatusCode.Locked, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.Locked,
                    Message = "Account Locked",
                    Data = new { Error = ex.Message }
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred",
                    Data = new { ExceptionMessage = ex.Message, StackTrace = ex.StackTrace }
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
                return Ok(new ReturnResponse<string>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "Generate new token",
                    Data = null
                });
            }
            catch(InvalidOperationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalida Operation",
                    Data = new { Error = ex.Message }
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred",
                    Data = new { ExceptionMessage = ex.Message, StackTrace = ex.StackTrace }
                });
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
                return Ok(new ReturnResponse<string>
                {
                    StatusCode = 200,
                    Message = "Successfully Logout",
                    Data = DateTime.UtcNow.ToString("o")
                });
            } 
            catch (InvalidTokenException ex)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    Message = "Invalid authentication token",
                    Data = new {Error = ex.Message}
                });
            }
            catch (InvalidCredentialsException ex)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Message = "Invalid credentials",
                    Data = new { Error = ex.Message }
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ReturnResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred",
                    Data = new { ExceptionMessage = ex.Message, StackTrace = ex.StackTrace }
                });
            }
        }

    }
}

