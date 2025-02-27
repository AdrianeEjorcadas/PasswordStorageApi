using PasswordStorageApi.Models;

namespace PasswordStorageApi.Repository.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel?>> GetAsync();
        Task<UserModel?> GetUserByIdAsync(int id);
        Task<UserModel> CreateAsync(UserModel model);
        Task<UserModel?> UpdateAsync(int userId, UserModel model);
        Task<UserModel?> DeleteAsync(int userId);
    }
}
