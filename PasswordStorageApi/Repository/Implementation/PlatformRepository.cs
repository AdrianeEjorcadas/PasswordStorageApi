using Microsoft.EntityFrameworkCore;
using PasswordStorageApi.Data;
using PasswordStorageApi.Models;
using PasswordStorageApi.Repository.Interface;

namespace PasswordStorageApi.Repository.Implementation
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly PasswordStorageDbContext _context;
        public PlatformRepository(PasswordStorageDbContext context)
        {
            _context = context;
        }
        public async Task<PlatformModel> AddPlatform(PlatformModel platform)
        {
            await _context.Platforms.AddAsync(platform);
            await _context.SaveChangesAsync();
            return platform;
        }

        public async Task<IEnumerable<PlatformModel?>> GetAllPlatforms()
        {
            return await _context.Platforms.ToListAsync();
        }

        public async Task<PlatformModel?> GetPlatformById(int platformId)
        {
            return await _context.Platforms.FindAsync(platformId);
        }

        public async Task<PlatformModel> UpdatePlatform(PlatformModel platform)
        {
            throw new NotImplementedException();
        }
    }
}
