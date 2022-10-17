using Services.Dto;

namespace Services.Abstract
{
    public interface IRolesService
    {
        // GET
        Task<List<RoleDto>> GetAllAsync();
        Task<RoleDto?> GetByIdAsync(Guid roleId);

        // POST
        Task<Guid> CreateAsync(RoleDto role);
        Task<Guid> UpdateByUserAsync(Guid userId, RoleDto role);

        // PUT
        Task<Guid> UpdateAsync(Guid roleId, RoleDto role);

        // DELETE
        Task<bool> DeleteAsync(RoleDto role);

        // EXISTS
        Task<bool> IsExistRoleAsync(Guid roleId);
        Task<bool> ISExistUserAsync(Guid userId);
    }
}
