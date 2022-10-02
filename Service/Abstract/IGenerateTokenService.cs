using Services.Dto;

namespace Services.Abstract
{
    public interface IGenerateTokenService
    {
        Task<string?> GenerateAccessTokenAsync(UserDto user);
        Task<string?> CreateRefreshTokenAsync(UserDto user);
        Task<UserDto?> GetUserByTokenAsync(string refreshToken);
        Task<TokenDto?> TokenAuthenticateAsync(UserDto user, string accessToken);
    }
}
