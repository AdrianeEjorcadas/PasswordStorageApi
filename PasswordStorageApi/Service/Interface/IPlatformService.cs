using PasswordStorageApi.Models;

namespace PasswordStorageApi.Service.Interface
{
    public interface IPlatformService
    {
        Task<IEnumerable<PlatformModel>> GetAllPlatforms();
        Task<PlatformModel> GetPlatformById(int platformId);
        Task<PlatformModel> AddPlatform(PlatformModel platform);
        Task<PlatformModel> UpdatePlatform(PlatformModel platform);
    }
}
