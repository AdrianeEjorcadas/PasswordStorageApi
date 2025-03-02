using PasswordStorageApi.Models;

namespace PasswordStorageApi.Service.Interface
{
    public interface IPasswordService
    {
        //Create password for specific platform
        Task<PasswordModel> CreateAsync(int platformId, PasswordModel password);

        // Get all users' password: both active and inactive; exclude deleted password
        Task<PasswordModel?> GetAllPasswordAsync(int userId);

        // Get all active password
        Task<PasswordModel?> GetActivePasswordAsync(int userId);

        // Get all inactive password
        Task<PasswordModel?> GetInactivePasswordAsync(int userId);

        // Get all password specifically for platform
        Task<PasswordModel?> GetPasswordByPlatformAsync(int userId, int platformId);

        // Update password
        Task<PasswordModel> UpdatePasswordAsync(int passwordId, PasswordModel password);

        // Delete password
        Task<PasswordModel> DeletePasswordAsync(int passwordId);

        // Get user chronological password history
        Task<PasswordModel> GetPasswordHistoryAsync(int userId);

        // Change the status of password of a specific platform.
        // If passwords enabled, all pw under of the same platform will be disabled
        Task<bool> ChangePassworStatusdAsync(int passwordId);

    }
}
