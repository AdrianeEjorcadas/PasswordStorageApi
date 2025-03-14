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

        public async Task<UserCredentialModel> CreateUserAsync(UserCredentialModel userModel)
        {
            await _context.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }
    }
}
