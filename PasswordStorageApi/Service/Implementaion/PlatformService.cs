using PasswordStorageApi.Models;
using PasswordStorageApi.Repository.Interface;
using PasswordStorageApi.Service.Interface;

namespace PasswordStorageApi.Service.Implementaion
{
    public class PlatformService : IPlatformService
    {
        private readonly IPlatformRepository _platformRepository;
        public PlatformService(IPlatformRepository platformRepository)
        {
            _platformRepository = platformRepository;
        }
        public async Task<PlatformModel> AddPlatform(PlatformModel platform)
        {
            return await _platformRepository.AddPlatform(platform);  
        }

        public async Task<IEnumerable<PlatformModel>> GetAllPlatforms()
        {
            return await _platformRepository.GetAllPlatforms();
        }

        public async Task<PlatformModel> GetPlatformById(int platformId)
        {
            return await _platformRepository.GetPlatformById(platformId);
        }

        public async Task<PlatformModel> UpdatePlatform(PlatformModel platform)
        {
            throw new NotImplementedException();
        }
    }
}
