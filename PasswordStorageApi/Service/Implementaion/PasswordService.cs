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

        public async Task<PasswordModel> CreateAsync(PasswordInputDTO passwordInput)
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

        public async Task<IEnumerable<PasswordModel>> GetPasswordHistoryAsync(int userId)
        {
            var passwords = await _passwordRepository.GetPasswordHistoryAsync(userId);

            foreach(var password in passwords)
            {
                password.EncryptedPassword = EncryptionHelper.Decrypt(password.EncryptedPassword);
            }
            return passwords;
        }

        public async Task<PasswordModel?> UpdatePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            var passwordToUpdate = await _passwordRepository.GetPasswordbyPwId(changePasswordDTO.PasswordId);

            if (passwordToUpdate == null)
                return null;

            var oldPassword = EncryptionHelper.Decrypt(passwordToUpdate.EncryptedPassword);
            if (oldPassword != changePasswordDTO.OldPassword)
                throw new ArgumentException("The old password is incorrect.");

            if (!changePasswordDTO.NewPassword.Equals(changePasswordDTO.ConfirmPassword))
                throw new ArgumentException("New password and confirm password do not match.");

            var newSalt = SaltHelper.GenerateSalt(16);
            string encryptedNewPassword = EncryptionHelper.Encrypt(changePasswordDTO.NewPassword, newSalt);

            var passwordModel = new PasswordModel
            {
                EncryptedPassword = encryptedNewPassword,
                Salt = Convert.ToBase64String(newSalt),
            };

            var updatedPassword = await _passwordRepository.UpdatePasswordAsync(passwordModel, changePasswordDTO.PasswordId);
            return updatedPassword;
        }

    }
}
