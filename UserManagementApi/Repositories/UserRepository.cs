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
        public Task<UserModel> CreateUserAsync(UserModel userModel)
        {
            throw new NotImplementedException();
        }
    }
}
