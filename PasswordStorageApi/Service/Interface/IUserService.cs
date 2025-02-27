using PasswordStorageApi.Models;

namespace PasswordStorageApi.Service.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel?>> GetAsync();
        Task<UserModel?> GetUserByIdAsync(int userId);
        Task<UserModel> CreateAsync(UserModel model);
        Task<UserModel?> UpdateAsync(int userId, UserModel model);
        Task<UserModel?> DeleteAsync(int userId);
    }
}
