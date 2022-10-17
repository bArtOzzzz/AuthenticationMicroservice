using Microsoft.EntityFrameworkCore;
using Repositories.Abstract;
using Repositories.Entities;
using Repositories.Context;

namespace Repositories
{
    public class GenerateTokenRepository : IGenerateTokenRepository
    {
        private readonly DataContext _context;

        // DB
        public GenerateTokenRepository(DataContext context) => _context = context;

        // GET
        public async Task<UserEntity?> GetUserByTokenAsync(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken!.Equals(refreshToken));
        }

        // POST
        public async Task<string?> CreateRefreshTokenAsync(UserEntity user)
        {
            UserEntity userEntity = new()
            {
                Id = user.Id,
                CreatedDate = user.CreatedDate,
                EmailAddress = user.EmailAddress,
                Password = user.Password,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                RoleId = user.RoleId,
                Username = user.Username
            };

            _context.Update(userEntity);
            await _context.SaveChangesAsync();
            return userEntity.RefreshToken;
        }
    }
}
