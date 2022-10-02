using Services.Dto;

namespace Services.Abstract
{
    public interface IAuthenticateService
    {
        // GET
        Task<UserDto?> AuthenticateAsync(string username, string password);
    }
}
