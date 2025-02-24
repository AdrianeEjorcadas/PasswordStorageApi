using Microsoft.AspNetCore.Mvc;
using PasswordStorageApi.Service.Implementaion;

namespace PasswordStorageApi.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
    }
}
