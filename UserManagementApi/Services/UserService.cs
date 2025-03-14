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
    }

}
