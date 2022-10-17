using Services.Dto;

namespace Services.Abstract
{
    public interface IUsersService
    {
        // GET
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(Guid userId);

        // PUT
        Task<Guid> UpdateAsync(Guid userId, UserDto user);
        Task<Guid> UpdateNameAsync(Guid userId, UserDto user);
        Task<Guid> UpdateEmailAsync(Guid userId, UserDto user);
        Task<Guid> UpdatePasswordAsync(Guid userId, UserDto user);
        Task<string> ResetPasswordAsync(Guid userId);

        // DELETE
        Task<bool> DeleteAsync(UserDto user);

        // EXISTS
        Task<bool> IsExistUserAsync(Guid userId);
        Task<bool> IsExistUserNameAsync(string username);
    }
}
