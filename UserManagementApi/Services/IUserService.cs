using UserManagementApi.DTO;
using UserManagementApi.Models;

namespace UserManagementApi.Services
{
    public interface IUserService
    {
        public Task<UserCredentialModel> CreateUserAsync(AddUserDTO addUserDTO);

        public Task<UserCredentialModel> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);

        public Task<bool> ForgotPasswordAsync(string email);
    }
}
