using Microsoft.EntityFrameworkCore;
using Repositories.Abstract;
using Repositories.Context;
using Repositories.Entities;

namespace Repositories
{
    public class AuthenticateRepository : IAuthenticateRepository
    {
        private readonly DataContext _context;

        public AuthenticateRepository(DataContext context) => _context = context;

        // GET
        public async Task<UserEntity?> AuthenticateAsync(string username, string password)
        {
            return await _context.Users.Where(u => u.Username.ToLower()
                                       .Equals(username))
                                       .FirstOrDefaultAsync();
        }
    }
}
