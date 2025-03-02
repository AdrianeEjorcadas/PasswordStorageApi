using PasswordStorageApi.Models;
using PasswordStorageApi.Repository.Interface;

namespace PasswordStorageApi.Repository.Implementation
{
    public class PasswordRepository : IPasswordRepository
    {
        public Task<bool> ChangePassworStatusdAsync(int passwordId)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordModel> CreateAsync(int platformId, PasswordModel password)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordModel> DeletePasswordAsync(int passwordId)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordModel?> GetActivePasswordAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordModel?> GetAllPasswordAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordModel?> GetInactivePasswordAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordModel?> GetPasswordByPlatformAsync(int userId, int platformId)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordModel> GetPasswordHistoryAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordModel> UpdatePasswordAsync(int passwordId, PasswordModel password)
        {
            throw new NotImplementedException();
        }
    }
}
