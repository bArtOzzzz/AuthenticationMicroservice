using Repositories.Entities;

namespace Repositories.Abstract
{
    public interface IUsersRepository
    {
        // GET
        Task<List<UserEntity>> GetAllAsync();
        Task<UserEntity?> GetByIdAsync(Guid userId);

        // PUT
        Task<Guid> UpdateAsync(Guid userId, UserEntity user);
        Task<Guid> UpdateNameAsync(Guid userId, UserEntity user);
        Task<Guid> UpdateEmailAsync(Guid userId, UserEntity user);
        Task<Guid> UpdatePasswordAsync(Guid userId, UserEntity user);
        Task<string> ResetPasswordAsync(Guid userId);

        // DELETE
        Task<bool> DeleteAsync(UserEntity user);

        // EXISTS
        Task<bool> IsExistUserAsync(Guid userId);
        Task<bool> IsExistUserNameAsync(string username);
    }
}
