using Microsoft.Extensions.Caching.Distributed;
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

        public async Task<bool> IsEmailExistingAsync(string email)
        {
            return await _userRepository.IsEmailExistingAsync(email);
        }

        public async Task<UserCredentialModel> CreateUserAsync(AddUserDTO addUserDTO)
        {
            var isUserExists = await _userRepository.IsUserExistAsync(addUserDTO.UserName);
            var isEmailExists = await _userRepository.IsEmailExistAsync(addUserDTO.Email);

            if (isUserExists)
            {
                throw new InvalidCredentialsException(ErrorMessages.UsernameExist);
            }

            if (isEmailExists) 
            {
                throw new InvalidCredentialsException(ErrorMessages.EmailExist);
            }

            if(!addUserDTO.Password.Equals(addUserDTO.ConfirmationPassword, StringComparison.Ordinal))
            {
                throw new InvalidCredentialsException(ErrorMessages.InvalidConfirmationCredential);
            }

            if(!CustomPasswordValidator.IsValid(addUserDTO.Password, out string errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            var salt = HashingHelper.GenerateSalt(16);
            var hashedPassword = HashingHelper.HashPassword(addUserDTO.Password, salt);

            var confirmationToken = HashingHelper.GenerateSalt(32);
            var hashedConfirmationToken = HashingHelper.HashToken(confirmationToken);

            var userModel = new UserCredentialModel
            {
                UserName = addUserDTO.UserName,
                Email = addUserDTO.Email,
                Password = hashedPassword,
                Salt = Convert.ToBase64String(salt),
                ConfirmationToken = hashedConfirmationToken
            };

            var result = await _userRepository.CreateUserAsync(userModel);

            #region move to confirmation email endpoint
            //Send link via email
            //var subject = "Your Account Has Been Created – Welcome to My Life!";
            //var body = $"<p>Hello {addUserDTO.UserName}! <br>" +
            //           "<p>Your account has been successfully created! You are now part of My Life community.</p> <br>" +
            //           "<p>Click the button below to login and start exploring:</p> <br>" +
            //           $"<a href='#'>{addUserDTO.UserName}</a>";

            //await _emailService.SendEmail(addUserDTO.Email, subject, body); 
            #endregion
            // Generate confirmation link
            var resetLinkHelper = new ResetLinkHelper(_httpContextAccessor);
            var confirmationLink = resetLinkHelper.GenerateConfirmationLink(hashedConfirmationToken);

            //Send link for email confirmation
            var subject = "Confirm Your Email - Action Required";
            var body = $"<p>Hi {addUserDTO.UserName}, <br>" +
                       "<p>Thank you for signing up! Please confirm your email address to activate your account.</p> <br>" +
                       $"<p>Click the link to verify your email: </p>" +
                       $"<a href='{confirmationLink}'>{confirmationLink}</a> <br>" +
                       "<p>If you didn't request this, please ignore this email.</p><br>" +
                       "<p>Your confirmation link will expire in <strong>24 hours</strong>, so verify your email soon!</p><br><br>" +
                       "<p>Best Zacari Softier Corp.</p>";

            await _emailService.SendEmail(addUserDTO.Email, subject, body);

            return result;
        }

        public async Task ValidateEmailTokenAsync(ConfirmationEmailDTO confirmationEmailDTO)
        {
             var result =  await _userRepository.ValidateEmailTokenAsync(confirmationEmailDTO);

            // Generate confirmation link
            var resetLinkHelper = new ResetLinkHelper(_httpContextAccessor);
            var loginLink = resetLinkHelper.GenerateLoginLink();

            //Send link via email
            var subject = "Your Account Has Been Created – Welcome to Zacari Softier!";
            var body = $"<p>Hello {result.UserName}! <br>" +
                       "<p>Your account has been successfully created! You are now part of Zacari Softier family.</p> <br>" +
                       "<p>Click the button below to login and start exploring:</p> <br>" +
                       $"<a href='{loginLink}'>{loginLink}</a> <br><br>" +
                       "<p>Best Zacari Softier Corp.</p>";

            await _emailService.SendEmail(result.Email, subject, body);
        }

        public async Task ResendEmailTokenAsync(ResendConfirmationDTO resendConfirmationDTO)
        {
            //generate new confirmation token
            var confirmationToken = HashingHelper.GenerateSalt(32);
            var hashedConfirmationToken = HashingHelper.HashToken(confirmationToken);

            // validate the email address and update the confirmation token
            var result = await _userRepository.ResendEmailTokenAsync(resendConfirmationDTO, hashedConfirmationToken);

            // Generate confirmation link
            var resetLinkHelper = new ResetLinkHelper(_httpContextAccessor);
            var confirmationLink = resetLinkHelper.GenerateConfirmationLink(hashedConfirmationToken);

            //Send link for email confirmation
            var subject = "Confirm Your Email - Action Required";
            var body = $"<p>Hi {result.UserName}, <br>" +
                       "<p>Thank you for signing up! Please confirm your email address to activate your account.</p> <br>" +
                       $"<p>Click the link to verify your email: </p>" +
                       $"<a href='{confirmationLink}'>{confirmationLink}</a> <br>" +
                       "<p>If you didn't request this, please ignore this email.</p><br>" +
                       "<p>Your confirmation link will expire in <strong>24 hours</strong>, so verify your email soon!</p><br><br>" +
                       "<p>Best Zacari Softier Corp.</p>";

            await _emailService.SendEmail(result.Email, subject, body);
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
                throw new InvalidCredentialsException(ErrorMessages.InvalidPassword);

            // compare current password and user input current password
            if (!oldPassword.Equals(hashedInputedOldPassword, StringComparison.Ordinal))
            {
                throw new InvalidCredentialsException(ErrorMessages.InvalidPassword);
            }

            // compare new password and confirmation password
            if (!changePasswordDTO.NewPassword.Equals(changePasswordDTO.ConfirmationPassword, StringComparison.Ordinal))
            {
                throw new  InvalidCredentialsException(ErrorMessages.InvalidConfirmationCredential);
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
                throw new Exception(ErrorMessages.TokenNotRegistered);

            // Generate reset link
            var resetLinkHelper = new ResetLinkHelper(_httpContextAccessor);
            var resetLink = resetLinkHelper.GenerateResetLink(tokenString);

            //Send link via email
            var subject = "Reset Your Password – Action Required";
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
                throw new InvalidTokenException(ErrorMessages.ResetTokenInvalid);

            // token comparison (Note. will replace stringcomparison to CryptographicOperations.FixedTimeEquals)
            if (!tokenRetrieved.Equals(hashedToken, StringComparison.Ordinal))
                throw new InvalidTokenException(ErrorMessages.ResetTokenInvalid);

            // expiration of token checking
            if (expirationDateTime == default || expirationDateTime < DateTime.UtcNow)
                throw new InvalidTokenException(ErrorMessages.ResetTokenInvalid);

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

        public async Task ValidateTokenAsync(string authToken)
        {
            var result = await _userRepository.GetAuthenticationTokenDetailsAsync(authToken);
            if (result is null)
                throw new RevokedTokenException(ErrorMessages.InvalidToken);

            if (result.IsRevoked)
                throw new RevokedTokenException(ErrorMessages.InvalidToken);

            if (result.AuthTokenExpiration < DateTime.UtcNow)
            {
                bool isRefreshExpired = await _userRepository.IsRefreshExpiredAsync(result.RefreshToken);
                if (!isRefreshExpired)
                {
                    throw new InvalidCredentialsException(ErrorMessages.ExpiredSession);
                }
            }
        }

        public async Task GenerateNewTokenAsync(string refreshToken)
        {
            var newRefToken = HashingHelper.GenerateSalt(32);
            var hashedNewRefToken = HashingHelper.HashToken(newRefToken);

            await _userRepository.RegenerateAuthTokenAsync(hashedNewRefToken, refreshToken);
        }

        public async Task LogoutAsync(AuthenticationTokenDetailsDTO tokenDetailsDTO)
        {
            //validate user
            var validateUser = await _userRepository.GetAuthenticationTokenDetailsAsync(tokenDetailsDTO.AuthToken);
            if (validateUser is null) 
                throw new InvalidTokenException(ErrorMessages.InvalidToken);

            var isUserIdValid = await _userRepository.IsUserIdExistAsync(validateUser.UserId);
            if (!isUserIdValid)
                throw new InvalidCredentialsException(ErrorMessages.InvalidUser);

            //revoked token
            await _userRepository.RevokedTokenAsync(tokenDetailsDTO);
        }

    }
}
