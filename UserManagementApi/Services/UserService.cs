using UserManagementApi.DTO;
using UserManagementApi.Helpers;
using UserManagementApi.Models;
using UserManagementApi.Repositories;

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
