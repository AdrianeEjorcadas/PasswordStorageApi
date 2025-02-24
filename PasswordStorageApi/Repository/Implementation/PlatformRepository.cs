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
            return await _context.Platforms
                .Where(e => !e.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PlatformModel?> GetPlatformById(int platformId)
        {
            return await _context.Platforms
                .Where(e => !e.IsDeleted && platformId == e.PlatformId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<PlatformModel> UpdatePlatform(int platformId, PlatformModel platform)
        {
            var platformToUpdate = await _context.Platforms
                .Where(e => !e.IsDeleted && platformId == e.PlatformId)
                .FirstOrDefaultAsync();


            platformToUpdate.PlatformName = platform.PlatformName;
            await _context.SaveChangesAsync();

            return platformToUpdate;
        }

        public async Task<PlatformModel> DeletePlatform(int platformId)
        {
            var platformToDelete = await _context.Platforms
                .Where(e => !e.IsDeleted && platformId == e.PlatformId)
                .FirstOrDefaultAsync();

            platformToDelete.IsDeleted = true;
            await _context.SaveChangesAsync();

            return platformToDelete;
        }
    }
}
