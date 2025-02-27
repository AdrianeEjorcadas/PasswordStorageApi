using Microsoft.AspNetCore.Mvc;
using PasswordStorageApi.Models;
using PasswordStorageApi.Service.Interface;

namespace PasswordStorageApi.Controller
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAsync()
        {
            try
            {
                var users = await _userService.GetAsync();
                if (!users.Any())
                {
                    return NotFound("No record found!");
                }
                return Ok(users);
            }
            catch (Exception ex) 
            { 
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserModel>> GetUserByID(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user is null){
                    return NotFound("User does not exist");
                }
                return Ok(user);
            } catch (Exception ex) {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("add-user")]
        public async Task<ActionResult<UserModel>> CreateAsync([FromBody]UserModel userModel)
        {
            try
            {
                var user = await _userService.CreateAsync(userModel);
                return CreatedAtAction(nameof(GetUserByID), new { userId = user.UserId }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("update-user/{userId}")]
        public async Task<ActionResult<UserModel>> UpdateAsync(int userId, UserModel userModel)
        {
            try
            {
                var user = await _userService.UpdateAsync(userId, userModel);
                if (user is null)
                {
                    return NotFound("User does not exist");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
