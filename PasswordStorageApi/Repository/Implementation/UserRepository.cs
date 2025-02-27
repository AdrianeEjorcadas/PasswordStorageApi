using Microsoft.EntityFrameworkCore;
using PasswordStorageApi.Data;
using PasswordStorageApi.Models;
using PasswordStorageApi.Repository.Interface;

namespace PasswordStorageApi.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly PasswordStorageDbContext _context;

        public UserRepository(PasswordStorageDbContext context)
        {
            _context = context;
        }
        public async Task<UserModel> CreateAsync(UserModel model)
        {
            await _context.Users.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<UserModel?> DeleteAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserModel?>> GetAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UserModel?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Where(e => e.UserId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<UserModel?> UpdateAsync(int userId, UserModel model)
        {
            var userToUpdate = await _context.Users
                .Where(e => e.UserId == userId)
                .FirstOrDefaultAsync();

            userToUpdate.UserName = model.UserName;
            await _context.SaveChangesAsync();

            return userToUpdate;

        }
    }
}
