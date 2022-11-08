using Repositories.Entities;

namespace Repositories.Abstract
{
    public interface IAuthenticateRepository
    {
        // GET
        Task<UserEntity?> AuthenticateAsync(string username);
    }
}
