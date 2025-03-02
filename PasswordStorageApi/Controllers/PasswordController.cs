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
        public async Task<ActionResult<PasswordModel>> CreateAsync(PasswordInputModel passwordInput)
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

    }
}
