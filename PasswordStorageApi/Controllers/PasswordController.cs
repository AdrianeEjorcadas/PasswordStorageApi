using Microsoft.AspNetCore.Mvc;
using PasswordStorageApi.DTO;
using PasswordStorageApi.Models;
using PasswordStorageApi.Service.Interface;

namespace PasswordStorageApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordService _passwordService;
        public PasswordController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpPost("add-password")]
        public async Task<ActionResult<PasswordModel>> CreateAsync([FromBody]PasswordInputDTO passwordInput)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdPassword = await _passwordService.CreateAsync(passwordInput);
                return Ok(createdPassword);
            } catch (NullReferenceException nuex)
            {
                return BadRequest(new { Message = nuex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("user-passwords/{userId:int}")]
        public async Task<ActionResult<IEnumerable<PasswordModel>>> GetAllPasswordAsync(int userId)
        {
            try
            {
                var passwords = await _passwordService.GetAllPasswordAsync(userId);
                if(!passwords.Any())
                    return NotFound("No password exist!");
                return Ok(passwords);
            }
            catch (Exception ex) 
            {
                return BadRequest(new {Message =  ex.Message});
            }
        }

        [HttpGet("active-passwords/{userId:int}")]
        public async Task<ActionResult<IEnumerable<PasswordModel?>>> GetActivePasswordAsync(int userId) 
        {
            try
            {
                var activePasswords = await _passwordService.GetActivePasswordAsync(userId);
                if (!activePasswords.Any())
                    return NotFound("No active password found!");
                return Ok(activePasswords);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("inactive-passwords/{userId:int}")]
        public async Task<ActionResult<IEnumerable<PasswordModel?>>> GetInactivePasswordAsync(int userId)
        {
            try
            {
                var inactivePasswords = await _passwordService.GetInactivePasswordAsync(userId);
                if (!inactivePasswords.Any())
                    return NotFound("No inactive passwords found");
                return Ok(inactivePasswords);
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("platform-passwords/{userId:int}/{platformId:int}")]
        public async Task<ActionResult<IEnumerable<PasswordModel?>>> GetPasswordByPlatform(int userId,  int platformId)
        {
            try
            {
                var passwords = await _passwordService.GetPasswordByPlatformAsync(userId, platformId);
                if (!passwords.Any())
                    return NotFound("No password found!");
                return Ok(passwords);
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message =  $"{ex.Message}" });
            }
        }

        [HttpPut("change-password")]
        public async Task<ActionResult<PasswordModel>> UpdatePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedPassword = await _passwordService.UpdatePasswordAsync(changePasswordDTO);
                return Ok(updatedPassword);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
