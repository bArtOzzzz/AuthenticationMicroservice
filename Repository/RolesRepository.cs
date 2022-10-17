using Microsoft.EntityFrameworkCore;
using Repositories.Abstract;
using Repositories.Entities;
using Repositories.Context;

namespace Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly DataContext _context;

        // DB
        public RolesRepository(DataContext context) => _context = context;

        // GET
        public async Task<List<RoleEntity>> GetAllAsync()
        {
            return await _context.Roles.AsNoTracking()
                                       .ToListAsync();
        }

        public async Task<RoleEntity?> GetByIdAsync(Guid roleId)
        {
            return await _context.Roles.Where(r => r.Id.Equals(roleId))
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync();
        }

        // POST
        public async Task<Guid> CreateAsync(RoleEntity role)
        {
            RoleEntity roleEntity = new()
            {
                CreatedDate = DateTime.Now,
                Role = role.Role
            };

            await _context.AddAsync(roleEntity);
            await _context.SaveChangesAsync();

            role.Id = roleEntity.Id;

            return roleEntity.Id;
        }

        // PUT
        public async Task<Guid> UpdateAsync(Guid roleId, RoleEntity role)
        {
            var currentRole = await _context.Roles.Where(r => r.Id.Equals(roleId))
                                                  .FirstOrDefaultAsync();

            currentRole!.Role = role.Role;

            _context.Update(currentRole);
            await _context.SaveChangesAsync();

            return roleId;
        }

        public async Task<Guid> UpdateByUserAsync(Guid UserId, RoleEntity role)
        {
            var currentUser = await _context.Users.Where(u => u.Id.Equals(UserId))
                                                  .FirstOrDefaultAsync();

            Guid newRole = await _context.Roles.Where(r => r.Role!.Equals(role.Role))
                                               .Select(p => p.Id)
                                               .FirstOrDefaultAsync();

            currentUser!.RoleId = newRole;

            _context.Update(currentUser);
            await _context.SaveChangesAsync();

            return UserId;
        }

        // DELETE
        public async Task<bool> DeleteAsync(RoleEntity role)
        {
            _context.Remove(role);
            var saved = _context.SaveChangesAsync();

            return await saved > 0;
        }

        //EXISTS
        public async Task<bool> IsExistRoleAsync(Guid roleId)
        {
            return await _context.Roles.FindAsync(roleId) != null;
        }

        public async Task<bool> ISExistUserAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId) != null;
        }
    }
}
