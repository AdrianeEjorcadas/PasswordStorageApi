using System.Text;
using UserManagementApi.DTO;
using UserManagementApi.Helpers;
using UserManagementApi.Models;
using UserManagementApi.Repositories;
using UserManagementApi.Utilities;

namespace UserManagementApi.Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserCredentialModel> CreateUserAsync(AddUserDTO addUserDTO)
        {
            var isUserExists = await _userRepository.IsUserExist(addUserDTO.UserName);
            var isEmailExists = await _userRepository.IsEmailExist(addUserDTO.Email);

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

        public async Task<UserCredentialModel> ChangePasswordAsync(string userId, ChangePasswordDTO changePasswordDTO)
        {
            // retrieve current password and salt
            var (oldPassword, salt) = await _userRepository.GetOldPasswordAndSaltAsync(userId);
            // convert retrieved salt into byte
            byte[] byteSalt = Encoding.UTF8.GetBytes(salt);
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

            return await _userRepository.ChangePasswordAsync(hashedNewPassword, salt);

        }
    }

}
