using UserManagementApi.DTO;
using UserManagementApi.Models;

namespace UserManagementApi.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> IsEmailExist(string email);

        public Task<bool> IsUserExist(string username);

        public Task<(string? oldPassword, string? salt)> GetOldPasswordAndSaltAsync(Guid userId);

        public Task<UserCredentialModel> CreateUserAsync(UserCredentialModel userModel);

        public Task<UserCredentialModel> ChangePasswordAsync(UserCredentialModel userCredentialModel, Guid userId);

    }
}
