using Services.Dto;

namespace Services.Abstract
{
    public interface IUsersService
    {
        // GET
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(Guid id);

        // PUT
        Task<Guid> UpdateAsync(Guid userId, UserDto user);
        Task<Guid> UpdateNameAsync(Guid id, UserDto user);
        Task<Guid> UpdateEmailAsync(Guid id, UserDto user);
        Task<Guid> UpdatePasswordAsync(Guid id, UserDto user);
        Task<string> ResetPasswordAsync(Guid id);

        // DELETE
        Task<bool> DeleteAsync(UserDto user);

        // EXISTS
        Task<bool> IsExistUserAsync(Guid id);
        Task<bool> IsExistUserNameAsync(string username);
    }
}
