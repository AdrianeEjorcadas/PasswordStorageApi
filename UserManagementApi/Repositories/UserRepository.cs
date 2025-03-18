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

        public async Task<bool> IsEmailExistAsync(string email)
        {
                return await _context.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsUserExistAsync(string username)
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

        public async Task<string?> GetSaltAsync(Guid userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Salt)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        public async Task<bool> IsUserLockedAsync(string email)
        {
            var result = await _context.Users
                .Where(u => u.Email == email)
                .Select(u => new { u.IsLocked, u.FailedLoginAttempts })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            int threshold = 5;

            if (result.IsLocked ||  result.FailedLoginAttempts >= threshold)
                return true;

            return false;
        }

        public async Task AddFailureCountAndLockedAccount(string email)
        {
            var result = await _context.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (result.FailedLoginAttempts < 5)
            {
                result.FailedLoginAttempts += 1;
            }
            else
            {
                result.IsLocked = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task ResetAccountLocked(string email)
        {
            var result = await _context.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            result.IsLocked = false;
            result.FailedLoginAttempts = 0;
            await _context.SaveChangesAsync();
        }

        public async Task<UserCredentialModel> CreateUserAsync(UserCredentialModel userModel)
        {
            await _context.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<(string? retrievedToken, Guid userId, DateTime expirationDateTime)> GetTokenDetailsAsync(string token)
        {
            var result = await _context.UserPasswordResets
                .Where(t => t.Token == token)
                .Select(t => new { t.Token, t.UserId, t.ExpirationDateTime })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return (null, Guid.Empty, DateTime.MinValue);
            }

            return (result.Token, result.UserId, result.ExpirationDateTime);
        }

        public async Task<(string? username, string? password, string? salt)> GetUserCredentialAsync(string email)
        {
            var result = await _context.Users
                .Where(u => u.Email == email)
                .Select(u => new { u.UserName, u.Password, u.Salt })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if(result == null)
            {
                return (null, null, null);
            }

            return (result.UserName, result.Password, result.Salt);
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

        public async Task<bool> CreateResetTokenAsync(string email, string hashedToken)
        {
            try
            {
                var userId = await _context.Users
                    .Where(u => u.Email == email)
                    .Select(u => u.Id)
                    .FirstOrDefaultAsync();

                var userPasswordReset = new UserPasswordResetModel
                {
                    UserId = userId,
                    Token = hashedToken
                };

                await _context.AddAsync(userPasswordReset);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        public async Task<UserCredentialModel> ResetPasswordAsync(UserCredentialModel userCredentialModel, Guid userId)
        {
            var passwordToUpdate = await _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            passwordToUpdate.Password = userCredentialModel.Password;
            await _context.SaveChangesAsync();

            return passwordToUpdate;   
        }
    }
}
