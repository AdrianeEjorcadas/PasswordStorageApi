using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using UserManagementApi.CustomExceptions;
using UserManagementApi.DTO;
using UserManagementApi.Helpers;
using UserManagementApi.Messages;
using UserManagementApi.Models;
using UserManagementApi.Repositories;
using UserManagementApi.Utilities;

namespace UserManagementApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly SmtpEmailHelper _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUserRepository userRepository,
                            SmtpEmailHelper emailService,
                            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserCredentialModel> CreateUserAsync(AddUserDTO addUserDTO)
        {
            var isUserExists = await _userRepository.IsUserExistAsync(addUserDTO.UserName);
            var isEmailExists = await _userRepository.IsEmailExistAsync(addUserDTO.Email);

            if (isUserExists)
            {
                throw new ArgumentException("The user name you entered is already associated with an account.\nPlease use a different user name or log in to your existing account.");
            }

            if (isEmailExists) 
            {
                throw new ArgumentException("The email address you entered is already associated with an account.\nPlease use a different email address or log in to your existing account.");
            }

            if(!addUserDTO.Password.Equals(addUserDTO.ConfirmationPassword, StringComparison.Ordinal))
            {
                throw new ArgumentException("Password and confirmation password do not match. Please ensure both fields are identical.");
            }

            if(!CustomPasswordValidator.IsValid(addUserDTO.Password, out string errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            var salt = HashingHelper.GenerateSalt(16);
            var hashedPassword = HashingHelper.HashPassword(addUserDTO.Password, salt);
            var userModel = new UserCredentialModel
            {
                UserName = addUserDTO.UserName,
                Email = addUserDTO.Email,
                Password = hashedPassword,
                Salt = Convert.ToBase64String(salt)
            };

            return await _userRepository.CreateUserAsync(userModel);
        }

        public async Task<UserCredentialModel> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            // retrieve current password and salt
            var (oldPassword, salt) = await _userRepository.GetOldPasswordAndSaltAsync(changePasswordDTO.UserId);
            // convert retrieved salt into byte
            byte[] byteSalt = Convert.FromBase64String(salt);
            // hash the user input current password
            var hashedInputedOldPassword = HashingHelper.HashPassword(changePasswordDTO.OldPassword, byteSalt);
            // hash the user input new password
            var hashedNewPassword = HashingHelper.HashPassword(changePasswordDTO.NewPassword, byteSalt);

            // check if null
            if (oldPassword is null || salt is null)
                throw new ArgumentException("The password you entered is incorrect.");

            // compare current password and user input current password
            if (!oldPassword.Equals(hashedInputedOldPassword, StringComparison.Ordinal))
            {
                throw new ArgumentException("The password you entered is incorrect.");
            }

            // compare new password and confirmation password
            if (!changePasswordDTO.NewPassword.Equals(changePasswordDTO.ConfirmationPassword, StringComparison.Ordinal))
            {
                throw new ArgumentException("The new password and confirmation password you entered do not match.");
            }

            var userModel = new UserCredentialModel
            {
                Password = hashedNewPassword,
                Salt = Convert.ToBase64String(byteSalt)
            };

            return await _userRepository.ChangePasswordAsync(userModel, changePasswordDTO.UserId);
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var isEmailExists = await _userRepository.IsEmailExistAsync(email);
            if (!isEmailExists)
                return false;
            
            // Generate reset token
            var token = HashingHelper.GenerateSalt(32);
            var hashedToken = HashingHelper.HashToken(token);
            var tokenString = Convert.ToBase64String(token);

            //Validate if the reset token is registered to the db
            var isTokenRegister = await _userRepository.CreateResetTokenAsync(email, hashedToken);
            if (!isTokenRegister)
                throw new Exception("Failed to register the reset token");

            // Generate reset link
            var resetLinkHelper = new ResetLinkHelper(_httpContextAccessor);
            var resetLink = resetLinkHelper.GenerateResetLink(tokenString);

            //Send link via email
            var subject = "Password Reset";
            var body = $"Click the link to reset your password: <a href='{resetLink}'>{resetLink}</a>";
            await _emailService.SendEmail(email, subject, body);

            return true;
        }

        public async Task<UserCredentialModel> ResetPasswordAsync(string token, ResetPasswordDTO resetPasswordDTO)
        {
            // retrieved token details
            var byteArgumentToken = Convert.FromBase64String(token);
            var hashedToken = HashingHelper.HashToken(byteArgumentToken);
            var (tokenRetrieved, userId, expirationDateTime) = await _userRepository.GetTokenDetailsAsync(hashedToken);

            //token checking
            if (tokenRetrieved is null)
                throw new TokenInvalidException(ErrorMessages.ResetTokenInvalid);

            // token comparison (Note. will replace stringcomparison to CryptographicOperations.FixedTimeEquals)
            if (!tokenRetrieved.Equals(hashedToken, StringComparison.Ordinal))
                throw new TokenInvalidException(ErrorMessages.ResetTokenInvalid);

            // expiration of token checking
            if (expirationDateTime == default || expirationDateTime < DateTime.UtcNow)
                throw new TokenInvalidException(ErrorMessages.ResetTokenInvalid);

            // password checking
            if (!CustomPasswordValidator.IsValid(resetPasswordDTO.NewPassword, out string errorMessage))
                throw new ArgumentException(errorMessage);

            if (!resetPasswordDTO.NewPassword.Equals(resetPasswordDTO.ConfirmPassword, StringComparison.Ordinal))
                throw new InvalidCredentialsException(ErrorMessages.InvalidConfirmationCredential);

            // hashing new password
            var userSalt = await _userRepository.GetSaltAsync(userId);
            var byteSalt = Convert.FromBase64String(userSalt);
            var hashedNewPassword = HashingHelper.HashPassword(resetPasswordDTO.NewPassword, byteSalt);

            var userCredsModel = new UserCredentialModel
            {
                Password = hashedNewPassword
            };

            var user = await _userRepository.ResetPasswordAsync(userCredsModel, userId);

            // reset account lock details
            await _userRepository.ResetAccountLocked(user.Email);

            return user;
        }

        public async Task<AuthenticationTokenDetailsDTO> LoginAsync(LoginDTO loginDTO)
        {
            // get user credential info
            var (userId, password, salt) = await _userRepository.GetUserCredentialAsync(loginDTO.EmailAddress);

            // check user credential values
            if (userId == Guid.Empty || password is null || salt is null)
                throw new ArgumentException(ErrorMessages.InvalidCredential);

            //check DTO password structure
            if (!CustomPasswordValidator.IsValid(loginDTO.Password, out string errorMessage))
                throw new ArgumentException(errorMessage);

            // hash DTO password 
            var byteSalt = Convert.FromBase64String(salt);
            var hashedInputPassword = HashingHelper.HashPassword(loginDTO.Password, byteSalt);

            // check if user accout was locked
            bool isUserLocked = await _userRepository.IsUserLockedAsync(loginDTO.EmailAddress);
            if (isUserLocked)
                throw new AccountLockedException(ErrorMessages.AccountLocked);

            // compare passwords
            if (!CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(password), Convert.FromBase64String(hashedInputPassword)))
            {
                // add login failure counter
                await _userRepository.AddFailureCountAndLockedAccount(loginDTO.EmailAddress);
                throw new InvalidCredentialsException(ErrorMessages.InvalidCredential);
            }

            // reset the login failure counter
            await _userRepository.ResetAccountLocked(loginDTO.EmailAddress);

            //generate authentication token and refresh token
            var authToken = HashingHelper.GenerateSalt(32);
            var refreshToken = HashingHelper.GenerateSalt(32);

            var hashedAuthToken = HashingHelper.HashToken(authToken);
            var hashedRefreshToken = HashingHelper.HashToken(refreshToken);

            var authTokenModel = new AuthenticationTokenModel
            {
                UserId = userId,
                Token = hashedAuthToken,
                RefreshToken = hashedRefreshToken
            };

            var result = await _userRepository.CreateAuthTokenAsync(authTokenModel);

            return new AuthenticationTokenDetailsDTO
            {
                AuthToken = hashedAuthToken,
                RefreshToken = hashedRefreshToken,
                AuthTokenExpiration = result.AuthTokenExpiration,
                RefreshTokenExpiration = result.RefreshTokenExpiration
            };
        }

        public async Task ValidateTokenAsync(string token)
        {
            var result = await _userRepository.GetAuthenticationTokenDetailsAsync(token);
            if (result is null)
                throw new TokenInvalidException(ErrorMessages.InvalidToken);

            if (result.IsRevoked)
                throw new TokenInvalidException(ErrorMessages.InvalidToken);

            if (result.AuthTokenExpiration > DateTime.UtcNow)
            {
                bool isRefreshExpired = await _userRepository.IsRefreshExpiredAsync(result.RefreshToken);
                if (isRefreshExpired)
                {
                    throw new TokenInvalidException(ErrorMessages.ExpiredSession);
                }
            }
        }

    }
}
