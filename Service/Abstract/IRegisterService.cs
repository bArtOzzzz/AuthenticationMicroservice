using Services.Dto;

namespace Services.Abstract
{
    public interface IRegisterService
    {
        // POST
        Task<Guid> RegisterAsync(UserDto user);

        // EXISTS
        Task<bool> ExistsAsync(string username);
    }
}
