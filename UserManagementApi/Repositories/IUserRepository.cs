using UserManagementApi.DTO;
using UserManagementApi.Models;

namespace UserManagementApi.Repositories
{
    public interface IUserRepository
    {
        public Task<UserModel> CreateUserAsync(UserModel userModel);
    }
}
