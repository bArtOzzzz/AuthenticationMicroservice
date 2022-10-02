using AuthenticationMicroservice.Models.Request;
using AuthenticationMicroservice.Models.Response;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Dto;

namespace AuthenticationMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly IMapper _mapper;

        public RolesController(IRolesService rolesService, IMapper mapper)
        {
            _rolesService = rolesService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetAllAsync()
        {
            if (!ModelState.IsValid)
                return NotFound();

            var roles = await _rolesService.GetAllAsync();

            return Ok(_mapper.Map<List<RoleResponse>>(roles));
        }

        [HttpGet("{roleId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetByIdAsync(Guid roleId)
        {
            bool isExist = await _rolesService.IsExistRoleAsync(roleId);

            if (!isExist || !ModelState.IsValid)
                return NotFound();

            var role = await _rolesService.GetByIdAsync(roleId);

            return Ok(_mapper.Map<RoleResponse>(role));
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        public async Task<ActionResult> CreateAsync(RoleModel role)
        {
            if (role == null || !ModelState.IsValid)
                return NotFound();

            var roleMap = _mapper.Map<RoleDto>(role);

            Guid roleId;

            if (roleMap != null)
                roleId = await _rolesService.CreateAsync(roleMap);
            else
                return NotFound();

            return Ok(roleId);
        }

        [HttpPut("updateUserRole/{userId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> UpdateByUserAsync(Guid userId, RoleModel role)
        {
            bool isExist = await _rolesService.ISExistUserAsync(userId);

            if (!isExist || role == null || !ModelState.IsValid)
                return NotFound();

            var roleMap = _mapper.Map<RoleDto>(role);

            if (roleMap != null)
                await _rolesService.UpdateByUserAsync(userId, roleMap);
            else
                return NotFound();

            return Ok(userId);
        }

        [HttpPut("{roleId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> UpdateAsync(Guid roleId, RoleModel role)
        {
            bool isExist = await _rolesService.IsExistRoleAsync(roleId);

            if (!isExist || role == null || !ModelState.IsValid)
                return NotFound();

            var roleMap = _mapper.Map<RoleDto>(role);

            if (roleMap != null)
                await _rolesService.UpdateAsync(roleId, roleMap);

            return Ok(roleId);
        }

        [HttpDelete("{roleId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteAsync(Guid roleId)
        {
            bool isExist = await _rolesService.IsExistRoleAsync(roleId);

            if (!isExist || !ModelState.IsValid)
                return NotFound();

            Task<RoleDto> roleToDelete = _rolesService.GetByIdAsync(roleId)!;

            if (await _rolesService.DeleteAsync(await roleToDelete) == false)
            {
                ModelState.AddModelError("", "Something went wrong deleting model");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
