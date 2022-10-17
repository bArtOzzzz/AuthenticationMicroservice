using Repositories.Abstract;
using Repositories.Entities;
using Services.Abstract;
using Services.Dto;
using AutoMapper;

namespace Services
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IMapper _mapper;

        public RolesService(IRolesRepository rolesRepository,
                            IMapper mapper)
        {
            _rolesRepository = rolesRepository;
            _mapper = mapper;
        }

        // GET
        public async Task<List<RoleDto>> GetAllAsync()
        {
            var roles = await _rolesRepository.GetAllAsync();

            return _mapper.Map<List<RoleDto>>(roles);
        }

        public async Task<RoleDto?> GetByIdAsync(Guid roleId)
        {
            var role = await _rolesRepository.GetByIdAsync(roleId);

            return _mapper.Map<RoleDto>(role);
        }

        // POST
        public async Task<Guid> CreateAsync(RoleDto role)
        {
            var roleMap = _mapper.Map<RoleEntity>(role);

            await _rolesRepository.CreateAsync(roleMap);
            role.Id = roleMap.Id;

            return roleMap.Id;
        }

        public async Task<Guid> UpdateByUserAsync(Guid userId, RoleDto role)
        {
            var roleMap = _mapper.Map<RoleEntity>(role);

            return await _rolesRepository.UpdateByUserAsync(userId, roleMap);
        }

        // PUT
        public async Task<Guid> UpdateAsync(Guid roleId, RoleDto role)
        {
            var roleMap = _mapper.Map<RoleEntity>(role);

            return await _rolesRepository.UpdateAsync(roleId, roleMap);
        }

        // DELETE
        public async Task<bool> DeleteAsync(RoleDto role)
        {
            var roleMap = _mapper.Map<RoleEntity>(role);

            return await _rolesRepository.DeleteAsync(roleMap);
        }

        public async Task<bool> IsExistRoleAsync(Guid roleId)
        {
            return await _rolesRepository.IsExistRoleAsync(roleId);
        }

        public async Task<bool> ISExistUserAsync(Guid userId)
        {
            return await _rolesRepository.ISExistUserAsync(userId);
        }
    }
}
