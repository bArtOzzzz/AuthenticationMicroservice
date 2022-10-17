using Microsoft.EntityFrameworkCore;
using Repositories.Abstract;
using Repositories.Entities;
using Repositories.Context;

namespace Repositories
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly DataContext _context;

        // DB
        public RegisterRepository(DataContext context) => _context = context;

        // POST
        public async Task<Guid> RegisterAsync(UserEntity user)
        {
            var defaultRole = await _context.Roles.Where(r => r.Role!.Equals("User"))
                                                  .FirstOrDefaultAsync();

            UserEntity userEntity = new()
            {
                CreatedDate = DateTime.Now,
                Username = user.Username,
                EmailAddress = user.EmailAddress,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                RoleId = defaultRole!.Id
            };

            await _context.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            user.Id = userEntity.Id;

            return user.Id;
        }

        // EXISTS
        public async Task<bool> ExistsAsync(string username)
        {
            return await _context.Users.FindAsync(username) != null;
        }
    }
}
