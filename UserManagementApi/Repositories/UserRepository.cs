using Microsoft.EntityFrameworkCore;
using UserManagementApi.Data;
using UserManagementApi.DTO;
using UserManagementApi.Models;

namespace UserManagementApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsEmailExist(string email)
        {
                return await _context.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsUserExist(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.UserName == username);
        }

        public async Task<(string? oldPassword, string? salt)> GetOldPasswordAndSaltAsync(Guid userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new {u.Password, u.Salt})
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var result = user != null ? (user.Password, user.Salt) : (null, null);

            return result;
        }

        public async Task<UserCredentialModel> CreateUserAsync(UserCredentialModel userModel)
        {
            await _context.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<UserCredentialModel> ChangePasswordAsync(UserCredentialModel userCredentialModel, Guid userId)
        {
            var passwordToUpdate = await _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            passwordToUpdate.Password = userCredentialModel.Password;
            passwordToUpdate.Salt = userCredentialModel.Salt;
            await _context.SaveChangesAsync();

            return userCredentialModel;
        }
    }
}
