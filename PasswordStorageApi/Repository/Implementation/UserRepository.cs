using PasswordStorageApi.Data;
using PasswordStorageApi.Models;
using PasswordStorageApi.Repository.Interface;

namespace PasswordStorageApi.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly PasswordStorageDbContext _context;

        public UserRepository(PasswordStorageDbContext context)
        {
            _context = context;
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
