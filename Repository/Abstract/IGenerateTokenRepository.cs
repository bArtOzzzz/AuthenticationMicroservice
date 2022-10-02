using Repositories.Entities;

namespace Repositories.Abstract
{
    public interface IGenerateTokenRepository
    {
        Task<string?> CreateRefreshTokenAsync(UserEntity user);
        Task<UserEntity?> GetUserByTokenAsync(string refreshToken);
    }
}
