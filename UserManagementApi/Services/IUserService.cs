using UserManagementApi.DTO;
using UserManagementApi.Models;

namespace UserManagementApi.Services
{
    public interface IUserService
    {
        public Task<UserModel> CreateUserAsync(AddUserDTO addUserDTO);
    }
}
