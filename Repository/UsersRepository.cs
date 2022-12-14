using Microsoft.EntityFrameworkCore;
using Repositories.Abstract;
using Repositories.Context;
using Repositories.Entities;

namespace Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _context;

        // DB
        public UsersRepository(DataContext context) => _context = context;

        // GET
        public async Task<List<UserEntity>> GetAllAsync()
        {
            return await _context.Users.AsNoTracking()
                                       .ToListAsync();
        }

        public async Task<UserEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Users.Where(u => u.Id.Equals(id))
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync();
        }

        // PUT
        public async Task<Guid> UpdateAsync(Guid userId, UserEntity user)
        {
            var currentUser = await _context.Users.Where(u => u.Id.Equals(userId))
                                                  .FirstOrDefaultAsync();

            currentUser.EmailAddress = user.EmailAddress;
            currentUser.Username = user.Username;
            currentUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Update(currentUser);
            await _context.SaveChangesAsync();

            return userId;
        }

        public async Task<Guid> UpdateNameAsync(Guid userId, UserEntity user)
        {
            var currentUser = await _context.Users.Where(u => u.Id.Equals(userId))
                                                  .FirstOrDefaultAsync();

            currentUser.Username = user.Username;

            _context.Update(currentUser);
            await _context.SaveChangesAsync();

            return userId;
        }

        public async Task<Guid> UpdateEmailAsync(Guid userId, UserEntity user)
        {
            var currentUser = await _context.Users.Where(u => u.Id.Equals(userId))
                                                  .FirstOrDefaultAsync();

            currentUser.EmailAddress = user.EmailAddress;

            _context.Update(currentUser);
            await _context.SaveChangesAsync();

            return userId;
        }

        public async Task<Guid> UpdatePasswordAsync(Guid userId, UserEntity user)
        {
            var currentUser = await _context.Users.Where(u => u.Id.Equals(userId))
                                                  .FirstOrDefaultAsync();

            currentUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Update(currentUser);
            await _context.SaveChangesAsync();

            return userId;
        }

        public async Task<string> ResetPasswordAsync(Guid id)
        {
            var currentUser = await _context.Users.Where(u => u.Id.Equals(id))
                                                  .FirstOrDefaultAsync();

            var newPassword = RandomString(12);

            currentUser.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            _context.Update(currentUser);
            await _context.SaveChangesAsync();

            return newPassword;
        }

        // DELETE
        public async Task<bool> DeleteAsync(UserEntity user)
        {
            _context.Remove(user);
            var saved = _context.SaveChangesAsync();

            return await saved > 0;
        }

        //___________________________________________
        // Generate random password (For reset)
        private Random random = new Random();

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random
                                        .Next(s.Length)])
                                        .ToArray());
        }

        // EXISTS
        public async Task<bool> IsExistUserAsync(Guid id)
        {
            return await _context.Users.AnyAsync(u => u.Id.Equals(id));
        }
        public async Task<bool> IsExistUserNameAsync(string username)
        {
            return await _context.Users.AnyAsync(f => f.Username.Equals(username));
        }
    }
}
