using AuthenticationMicroservice.Models.Request;
using AuthenticationMicroservice.Models.Response;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Dto;
using System.Security.Claims;

namespace AuthenticationMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public UsersController(IUsersService usersService,
                               IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetAllAsync()
        {
            if (!ModelState.IsValid)
                return NotFound();

            var users = await _usersService.GetAllAsync();

            return Ok(_mapper.Map<List<UserResponse>>(users));
        }

        [Authorize(Roles = "User, Administrator")]
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetByIdAsync(Guid userId)
        {
            bool isExist = await _usersService.IsExistUserAsync(userId);

            if (!isExist || !ModelState.IsValid)
                return NotFound();

            // Check user for valid
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var currentUserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (new Guid(currentUserId) != userId)
                return BadRequest("You do not have premission for this action");

            var user = await _usersService.GetByIdAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserResponse>(user));
        }

        [Authorize(Roles = "User, Administrator")]
        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateAsync(Guid userId, UserModel user)
        {
            bool isExistName = await _usersService.IsExistUserNameAsync(user.Username);
            bool isExist = await _usersService.IsExistUserAsync(userId);

            if (!isExist || user == null || !ModelState.IsValid)
                return NotFound("User does not exist");

            if (isExistName)
                return NotFound("Username already exist");

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var currentUserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userMap = _mapper.Map<UserDto>(user);

            if (userMap == null)
                return NotFound("User does not exist");

            if (new Guid(currentUserId) != userId)
                return BadRequest("You do not have premission for this action");

            await _usersService.UpdateAsync(userId, userMap);

            return Ok(userId);
        }

        [Authorize(Roles = "User, Administrator")]
        [HttpPut("name/{id}")]
        public async Task<ActionResult> UpdateNameAsync(Guid id, UserNameModel user)
        {
            bool isExist = await _usersService.IsExistUserAsync(id);
            bool isExistName = await _usersService.IsExistUserNameAsync(user.Username);

            if(!isExist || !ModelState.IsValid)
                return NotFound("User does not exist");

            if (isExistName)
                return NotFound("Username already exist");

            var userMap = _mapper.Map<UserDto>(user);

            if (userMap == null)
                return NotFound("User does not exist");

            await _usersService.UpdateNameAsync(id, userMap);

            return Ok(id);
        }

        [Authorize(Roles = "User, Administrator")]
        [HttpPut("email/{id}")]
        public async Task<ActionResult> UpdateEmailAsync(Guid id, UserEmailModel user)
        {
            bool isExist = await _usersService.IsExistUserAsync(id);

            if (!isExist || !ModelState.IsValid)
                return NotFound("User does not exist");

            var userMap = _mapper.Map<UserDto>(user);

            if (userMap == null)
                return NotFound("User does not exist");

            await _usersService.UpdateEmailAsync(id, userMap);

            return Ok(id);
        }

        [Authorize(Roles = "User, Administrator")]
        [HttpPut("password/{id}")]
        public async Task<ActionResult> UpdatePasswordAsync(Guid id, UserPasswordModel user)
        {
            // TODO: Add check on the same password 
            bool isExist = await _usersService.IsExistUserAsync(id);

            if (!isExist || !ModelState.IsValid)
                return NotFound("User does not exist");

            var userMap = _mapper.Map<UserDto>(user);

            if (userMap == null)
                return NotFound("User does not exist");

            await _usersService.UpdatePasswordAsync(id, userMap);

            return Ok(id);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("reset/{id}")]
        public async Task<ActionResult> ResetPasswordAsync(Guid id)
        {
            bool isExist = await _usersService.IsExistUserAsync(id);

            if (!isExist || !ModelState.IsValid)
                return NotFound("User does not exist");

            var autogeneratedPassword = await _usersService.ResetPasswordAsync(id);

            return Ok(autogeneratedPassword);
        }

        [Authorize(Roles = "User, Administrator")]
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteAsync(Guid userId)
        {
            bool isExist = await _usersService.IsExistUserAsync(userId);

            if (!isExist || !ModelState.IsValid)
                return NotFound("User does not exist");

            Task<UserDto> userToDelete = _usersService.GetByIdAsync(userId)!;

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var currentUserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (new Guid(currentUserId) != userId)
                return BadRequest("You do not have premission for this action");

            if (await _usersService.DeleteAsync(await userToDelete) == false)
            {
                ModelState.AddModelError("", "Something went wrong deleting user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
