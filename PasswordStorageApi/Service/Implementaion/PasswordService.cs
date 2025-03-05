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
      
        public async Task<IEnumerable<PasswordModel?>> GetAllPasswordAsync(int userId)
        {
            var passwords = await _passwordRepository.GetAllPasswordAsync(userId);

            foreach(var password in passwords)
            {
                password.EncryptedPassword = EncryptionHelper.Decrypt(password.EncryptedPassword);
            }
            return passwords;
        }

        public async Task<IEnumerable<PasswordModel?>> GetActivePasswordAsync(int userId)
        {
            var activePasswords = await _passwordRepository.GetActivePasswordAsync(userId);

            foreach (var password in activePasswords) 
            {
                password.EncryptedPassword = EncryptionHelper.Decrypt(password.EncryptedPassword);
            }

            return activePasswords;

        }

        public async Task<IEnumerable<PasswordModel?>> GetInactivePasswordAsync(int userId)
        {
            var inactivePasswords = await _passwordRepository.GetInactivePasswordAsync(userId);

            foreach(var password in inactivePasswords)
            {
                password.EncryptedPassword = EncryptionHelper.Decrypt(password.EncryptedPassword);
            }

            return inactivePasswords;
        }

        public async Task<IEnumerable<PasswordModel?>> GetPasswordByPlatformAsync(int userId, int platformId)
        {
            var passwords = await _passwordRepository.GetPasswordByPlatformAsync(userId, platformId);

            foreach(var password in passwords)
            {
                password.EncryptedPassword = EncryptionHelper.Decrypt(password.EncryptedPassword);
            }
            return passwords;
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
