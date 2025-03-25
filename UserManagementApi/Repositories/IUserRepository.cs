using UserManagementApi.DTO;
using UserManagementApi.Models;

namespace UserManagementApi.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> IsEmailExistAsync(string email);

        public Task<bool> IsUserExistAsync(string username);

        public Task<(string? oldPassword, string? salt)> GetOldPasswordAndSaltAsync(Guid userId);

        public Task AddFailureCountAndLockedAccount(string email);

        public Task ResetAccountLocked(string email);

        public Task<string?> GetSaltAsync(Guid userId);

        public Task<bool> IsUserLockedAsync(string email);

        public Task<(Guid userId, string password, string? salt)> GetUserCredentialAsync(string email); 

        public Task<(string? retrievedToken, Guid userId, DateTime expirationDateTime)> GetTokenDetailsAsync(string token);

        Task <ValidateAuthToken> GetAuthenticationTokenDetailsAsync(string token);

        Task<bool> IsRefreshExpiredAsync(string refToken);

        public Task<UserCredentialModel> CreateUserAsync(UserCredentialModel userModel);

        public Task<UserCredentialModel> ChangePasswordAsync(UserCredentialModel userCredentialModel, Guid userId);

        public Task<bool> CreateResetTokenAsync(string email, string hashedToken);

        public Task<UserCredentialModel> ResetPasswordAsync(UserCredentialModel userCredentialModel, Guid userId);

        public Task<AuthenticationTokenModel> CreateAuthTokenAsync(AuthenticationTokenModel authenticationTokenModel);

        public Task<AuthenticationTokenModel> RegenerateAuthTokenAsync(string refToken);


    }
}
