using Microsoft.EntityFrameworkCore;
using PasswordStorageApi.Data;
using PasswordStorageApi.Models;
using PasswordStorageApi.Repository.Interface;
using PasswordStorageApi.Service.Logging;

namespace PasswordStorageApi.Repository.Implementation
{
    public class PasswordRepository : IPasswordRepository
    {
        private readonly PasswordStorageDbContext _context;
        private readonly DatabaseLogger _logger;
        public PasswordRepository(DatabaseLogger logger,  PasswordStorageDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<bool> ChangePassworStatusdAsync(int passwordId)
        {
            var passwordToUpdate = await _context.Passwords
                .Where(e => e.PasswordId == passwordId)
                .FirstOrDefaultAsync();

            passwordToUpdate.IsActive = !passwordToUpdate.IsActive;
            await _context.SaveChangesAsync();

            return passwordToUpdate.IsActive;
        }

        public async Task<PasswordModel> CreateAsync(PasswordModel password)
        {
            await _context.Passwords.AddAsync(password);
            await _context.SaveChangesAsync();
            return password;
        }

        public async Task<PasswordModel> DeletePasswordAsync(int passwordId)
        {
            var passwordToDelete = await _context.Passwords
                .Where(e => e.PasswordId == passwordId)
                .FirstOrDefaultAsync();

            passwordToDelete.IsDeleted = true;
            await _context.SaveChangesAsync();

            return passwordToDelete;
        }

        public async Task<IEnumerable<PasswordModel?>> GetActivePasswordAsync(int userId)
        {
            return await _context.Passwords
                .Where (e => e.UserId == userId && e.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<PasswordModel?>> GetAllPasswordAsync(int userId)
        {
            return await _context.Passwords
                .Where(e => e.UserId == userId && !e.IsExpired)
                .ToListAsync();
        }

        public async Task<IEnumerable<PasswordModel?>> GetInactivePasswordAsync(int userId)
        {
            return await _context.Passwords
                .Where(e => e.UserId == userId && !e.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<PasswordModel?>> GetPasswordByPlatformAsync(int userId, int platformId)
        {
            return await _context.Passwords
                .Where(e => e.UserId == userId && e.PlatformId == platformId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PasswordModel>> GetPasswordHistoryAsync(int userId)
        {
            return await _context.Passwords
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<PasswordModel> UpdatePasswordAsync(int passwordId, PasswordModel password)
        {
            throw new NotImplementedException();
        }
    }
}
