using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using UserManagementApi.CustomExceptions;
using UserManagementApi.Data;
using UserManagementApi.DTO;
using UserManagementApi.Messages;
using UserManagementApi.Models;

namespace UserManagementApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;


        public UserRepository(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public async Task<bool> IsEmailExistingAsync(string email)
        {
            return await _context.Users
            .AnyAsync(u => u.Email == email);
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

        public async Task<bool> IsUserIdExistAsync(Guid id)
        {
            return await _context.Users
                .AnyAsync(u => u.Id == id);
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

        public async Task<bool> IsRefreshExpiredAsync(string refToken)
        {
            var result = await _context.AuthenticationTokens
                .Where(a => a.RefreshToken == refToken)
                .Select(a => new { a.RefreshTokenExpiration })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result.RefreshTokenExpiration > DateTime.UtcNow)
            {
                return false;
            }

            return true;
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

        public async Task<(Guid userId, string? password, string? salt)> GetUserCredentialAsync(string email)
        {
            var result = await _context.Users
                .Where(u => u.Email == email)
                .Select(u => new { u.Id, u.Password, u.Salt })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if(result == null)
            {
                return (Guid.Empty, null, null);
            }

            return (result.Id, result.Password, result.Salt);
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

        public async Task<AuthenticationTokenModel> CreateAuthTokenAsync(AuthenticationTokenModel authenticationTokenModel)
        {
            await _context.AddAsync(authenticationTokenModel);
            await _context.SaveChangesAsync();
            return authenticationTokenModel;
        
        
        }

        public async Task<AuthenticationTokenModel> RegenerateAuthTokenAsync(string newRefToken, string refreshToken)
        {
            var result = await _context.AuthenticationTokens
                .Where(t => t.RefreshToken == refreshToken)
                .FirstOrDefaultAsync();

            if (result is null)
            {
                throw new InvalidOperationException("Refresh token not found!.");
            }

            result.RefreshToken = newRefToken;
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<ValidateAuthTokenDTO> GetAuthenticationTokenDetailsAsync(string token)
        {
            var result = await _context.AuthenticationTokens
                .Where(a => a.Token == token)
                .Select(a => new {a.UserId, a.RefreshToken, a.AuthTokenExpiration, a.RefreshTokenExpiration, a.IsRevoked })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result is null)
                return null;

            return new ValidateAuthTokenDTO
            {
                UserId = result.UserId,
                RefreshToken = result.RefreshToken,
                AuthTokenExpiration = result.AuthTokenExpiration,
                RefreshTokenExpiration = result.RefreshTokenExpiration,
                IsRevoked = result.IsRevoked
            };
        }

        public async Task RevokedTokenAsync(AuthenticationTokenDetailsDTO tokenDetailsDTO)
        {
            var result = await _context.AuthenticationTokens
                .Where(a => a.Token == tokenDetailsDTO.AuthToken && a.RefreshToken == tokenDetailsDTO.RefreshToken )
                .FirstOrDefaultAsync();

            if (result is null)
                throw new InvalidTokenException(ErrorMessages.InvalidToken);

            result.RevokeAt = DateTime.UtcNow;
            result.IsRevoked = true;
            await _context.SaveChangesAsync();
        }

        public async Task<UserCredentialModel> ValidateEmailTokenAsync(ConfirmationEmailDTO confirmationEmailDTO)
        {
            var result = await _context.Users.IgnoreQueryFilters()
                .Where(u => !u.IsEmailConfirmed && u.ConfirmationToken == confirmationEmailDTO.ConfirmationToken && u.ConfirmationTokenExpiration > DateTime.UtcNow && !u.IsDeleted)
                .FirstOrDefaultAsync();

            if (result is null)
                throw new RevokedTokenException(ErrorMessages.ConfirmationTokenInvalid);

            result.IsEmailConfirmed = true;
            result.ConfirmationToken = null;
            result.ConfirmationTokenExpiration = null;
            await _context.SaveChangesAsync();

            return result;
        }

       public async Task<UserCredentialModel> ResendEmailTokenAsync(ResendConfirmationDTO resendConfirmationDTO, string confirmationToken)
        {
            
            // cache key
            var cacheKey = $"User_{resendConfirmationDTO.Email}";

            // get the JSON data using cache key
            var cacheUserJson = await _cache.GetStringAsync(cacheKey);

            // get the data from cache or database
            UserCredentialModel? result = cacheUserJson != null 
                ? JsonSerializer.Deserialize<UserCredentialModel>(cacheUserJson) 
                : await _context.Users.IgnoreQueryFilters()
                .Where(u => !u.IsEmailConfirmed && resendConfirmationDTO.Email == u.Email)
                .FirstOrDefaultAsync();

            if (result is null)
                throw new RevokedTokenException(ErrorMessages.ConfirmationTokenInvalid);

            result.ConfirmationToken = confirmationToken;
            result.ConfirmationTokenExpiration = DateTime.UtcNow.AddDays(1);
            await _context.SaveChangesAsync();

            //update cache
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return result;
        }


    }
}
