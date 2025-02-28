using PasswordStorageApi.Models;
using PasswordStorageApi.Repository.Implementation;
using PasswordStorageApi.Repository.Interface;
using PasswordStorageApi.Service.Interface;

namespace PasswordStorageApi.Service.Implementaion
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserModel> CreateAsync(UserModel model)
        {
            return await _userRepository.CreateAsync(model);
        }

        public async Task<UserModel?> DeleteAsync(int userId)
        {
            return await _userRepository.DeleteAsync(userId);
        }

        public async Task<IEnumerable<UserModel?>> GetAsync()
        {
            return await _userRepository.GetAsync();
        }

        public async Task<UserModel?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<UserModel?> UpdateAsync(int userId, UserModel model)
        {
            return await _userRepository.UpdateAsync(userId, model);
        }

        public async Task<UserModel> UpdateUserStatusAsync(int userId)
        {
            return await _userRepository.UpdateUserStatusAsync(userId);
        }
    }
}
