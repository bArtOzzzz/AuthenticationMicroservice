using Repositories.Entities;

namespace Repositories.Abstract
{
    public interface IUsersRepository
    {
        // GET
        Task<List<UserEntity>> GetAllAsync();
        Task<UserEntity?> GetByIdAsync(Guid id);

        // PUT
        Task<Guid> UpdateAsync(Guid userId, UserEntity user);
        Task<Guid> UpdateNameAsync(Guid id, UserEntity user);
        Task<Guid> UpdateEmailAsync(Guid id, UserEntity user);
        Task<Guid> UpdatePasswordAsync(Guid id, UserEntity user);
        Task<string> ResetPasswordAsync(Guid id);

        // DELETE
        Task<bool> DeleteAsync(UserEntity user);

        // EXISTS
        Task<bool> IsExistUserAsync(Guid id);
        Task<bool> IsExistUserNameAsync(string username);
    }
}
