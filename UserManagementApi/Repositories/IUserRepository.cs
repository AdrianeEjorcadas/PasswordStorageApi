using UserManagementApi.DTO;
using UserManagementApi.Models;

namespace UserManagementApi.Repositories
{
    public interface IUserRepository
    {

        Task<bool> IsEmailExistingAsync(string email);

        Task<bool> IsEmailExistAsync(string email);

        Task<bool> IsUserExistAsync(string username);

        Task<bool> IsUserIdExistAsync(Guid id);

        Task<UsernameEmailDTO> GetUserData(Guid userId);

        Task<(string? oldPassword, string? salt)> GetOldPasswordAndSaltAsync(Guid userId);

        Task AddFailureCountAndLockedAccount(string email);

        Task ResetAccountLocked(string email);

        Task<string?> GetSaltAsync(Guid userId);

        Task<bool> IsUserLockedAsync(string email);

        Task<UserDetailsDTO> GetUserByAsync(AuthenticationTokenDetailsDTO authenticationTokenDetails);

        Task<(Guid userId, string password, string? salt)> GetUserCredentialAsync(string email);

        Task<(string? retrievedToken, Guid userId, DateTime expirationDateTime)> GetTokenDetailsAsync(string token);

        Task <ValidateAuthTokenDTO> GetAuthenticationTokenDetailsAsync(string token);

        Task<bool> IsRefreshExpiredAsync(string refToken);

        Task<UserCredentialModel> CreateUserAsync(UserCredentialModel userModel);

        Task<UserCredentialModel> ChangePasswordAsync(UserCredentialModel userCredentialModel, Guid userId);

        Task<bool> CreateResetTokenAsync(string email, string hashedToken);

        Task<UserCredentialModel> ResetPasswordAsync(UserCredentialModel userCredentialModel, Guid userId);

        Task<AuthenticationTokenModel> CreateAuthTokenAsync(AuthenticationTokenModel authenticationTokenModel);

        Task<AuthenticationTokenModel> RegenerateAuthTokenAsync(string newRefToken, string refreshToken);

        Task RevokedTokenAsync(AuthenticationTokenDetailsDTO tokenDetailsDTO);

        Task<UserCredentialModel> ValidateEmailTokenAsync(ConfirmationEmailDTO confirmationEmailDTO);

        Task<UserCredentialModel> ResendEmailTokenAsync(ResendConfirmationDTO resendConfirmationDTO, string confirmationToken);

    }
}
