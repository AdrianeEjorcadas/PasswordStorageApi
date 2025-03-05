using PasswordStorageApi.DTO;
using PasswordStorageApi.Helpers;
using PasswordStorageApi.Models;
using PasswordStorageApi.Repository.Interface;
using PasswordStorageApi.Service.Interface;

namespace PasswordStorageApi.Service.Implementaion
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordRepository _passwordRepository;
        public PasswordService(IPasswordRepository passwordRepository)
        {
            _passwordRepository = passwordRepository;
        }
        public Task<bool> ChangePassworStatusdAsync(int passwordId)
        {
            throw new NotImplementedException();
        }

        public async Task<PasswordModel> CreateAsync(PasswordInputModel passwordInput)
        {
            var salt = SaltHelper.GenerateSalt(16);
            string encryptedPassword = EncryptionHelper.Encrypt(passwordInput.PlainTextPassword, salt);

            var passwordModel = new PasswordModel
            {
                UserId = passwordInput.UserId,
                PlatformId = passwordInput.PlatformId,
                EncryptedPassword = encryptedPassword,
                Salt = Convert.ToBase64String(salt)
            };

            return await _passwordRepository.CreateAsync(passwordModel);
        }

        public Task<PasswordModel> DeletePasswordAsync(int passwordId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PasswordModel?>> GetActivePasswordAsync(int userId)
        {
            return await _passwordRepository.GetActivePasswordAsync(userId);
        }

        public async Task<IEnumerable<PasswordModel?>> GetAllPasswordAsync(int userId)
        {
            return await _passwordRepository.GetAllPasswordAsync(userId);
        }

        public Task<IEnumerable<PasswordModel?>> GetInactivePasswordAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PasswordModel?>> GetPasswordByPlatformAsync(int userId, int platformId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PasswordModel>> GetPasswordHistoryAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordModel> UpdatePasswordAsync(int passwordId, PasswordModel password)
        {
            throw new NotImplementedException();
        }
    }
}
