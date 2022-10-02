using Repositories.Entities;

namespace Repositories.Abstract
{
    public interface IRegisterRepository
    {
        // POST
        Task<Guid> RegisterAsync(UserEntity user);

        // EXISTS
        Task<bool> ExistsAsync(string username);
    }
}
