using PasswordStorageApi.DTO;
using PasswordStorageApi.Models;

namespace PasswordStorageApi.Repository.Interface
{
    public interface IPasswordRepository
    {
        //Create password for specific platform
        Task<PasswordModel> CreateAsync(PasswordModel passwordModel);

        // Get all of specific users' password for all platform: both active and inactive; exclude deleted and expired password 
        Task<IEnumerable<PasswordModel?>> GetAllPasswordAsync(int userId);

        // Get all active password
        Task<IEnumerable<PasswordModel?>> GetActivePasswordAsync(int userId);

        // Get all inactive password
        Task<IEnumerable<PasswordModel?>> GetInactivePasswordAsync(int userId);

        // Get all password specifically for platform
        Task<IEnumerable<PasswordModel?>> GetPasswordByPlatformAsync(int userId, int platformId);

        // Update password
        Task<PasswordModel> UpdatePasswordAsync(ChangePasswordDTO changePasswordDTO);

        // Delete password
        Task <PasswordModel> DeletePasswordAsync(int passwordId);

        // Get user chronological password history
        Task<IEnumerable<PasswordModel>> GetPasswordHistoryAsync(int userId);

        // Change the status of password of a specific platform.
        // If passwords enabled, all pw under of the same platform will be disabled
        Task<bool> ChangePassworStatusdAsync(int passwordId);

    } 
}
