using PasswordStorageApi.Models;
using PasswordStorageApi.Repository.Implementation;
using PasswordStorageApi.Service.Interface;

namespace PasswordStorageApi.Service.Implementaion
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<UserModel> CreateAsync(UserModel model)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> DeleteAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> UpdateAsync(int userId, UserModel model)
        {
            throw new NotImplementedException();
        }
    }
}
