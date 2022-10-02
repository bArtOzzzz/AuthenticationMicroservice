using Repositories.Entities;

namespace Repositories.Abstract
{
    public interface IRolesRepository
    {
        // GET
        Task<List<RoleEntity>> GetAllAsync();
        Task<RoleEntity?> GetByIdAsync(Guid id);

        // POST
        Task<Guid> CreateAsync(RoleEntity role);
        Task<Guid> UpdateByUserAsync(Guid userId, RoleEntity role);

        // PUT
        Task<Guid> UpdateAsync(Guid roleId, RoleEntity role);

        // DELETE
        Task<bool> DeleteAsync(RoleEntity role);

        // EXISTS
        Task<bool> IsExistRoleAsync(Guid roleId);
        Task<bool> ISExistUserAsync(Guid userId);
    }
}
