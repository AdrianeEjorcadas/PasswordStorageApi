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
        public async Task<ActionResult<PasswordModel>> CreateAsync([FromBody]PasswordInputModel passwordInput)
        {
            try
            {
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
                return Ok(passwords);
            }
            catch (NullReferenceException nuex)
            {
                return NotFound("No passwords exist!");
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
                return Ok(activePasswords);
            } catch(NullReferenceException nex)
            {
                return NotFound("No active password found!");
            } catch (Exception ex)
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
                return Ok(inactivePasswords);
            } catch (NullReferenceException nex)
            {
                return NotFound("No inactive password found!");
            } catch(Exception ex)
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
                return Ok(passwords);
            }
            catch (NullReferenceException nex) 
            {
                return NotFound("No password found!");
            } catch(Exception ex)
            {
                return BadRequest(new { Message =  $"{ex.Message}" });
            }
        }

    }
}
