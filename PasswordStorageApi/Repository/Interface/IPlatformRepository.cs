using PasswordStorageApi.Models;

namespace PasswordStorageApi.Repository.Interface
{
    public interface IPlatformRepository
    {
        Task<IEnumerable<PlatformModel?>> GetAllPlatforms();
        Task<PlatformModel?> GetPlatformById(int platformId);
        Task<PlatformModel> AddPlatform(PlatformModel platform);
        Task<PlatformModel> UpdatePlatform(int platformId, PlatformModel platform);
        Task<PlatformModel> DeletePlatform(int platformId);
    }
}
