using AuthenticationMicroservice.Models.Response;
using AuthenticationMicroservice.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Services.Abstract;
using Services.Dto;
using AutoMapper;

namespace AuthenticationMicroservice.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
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
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> GetAllAsync()
        {
            if (!ModelState.IsValid)
                return NotFound();

            var users = await _usersService.GetAllAsync();

            return Ok(_mapper.Map<List<UserResponse>>(users));
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "User, Administrator")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> GetByIdAsync(Guid userId)
        {
            bool isExist = await _usersService.IsExistUserAsync(userId);

            if (!isExist || !ModelState.IsValid)
                return NotFound();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var currentUserId = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (new Guid(currentUserId!) != userId)
                return Unauthorized("You do not have premission for this action");

            var user = await _usersService.GetByIdAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserResponse>(user));
        }

        [HttpPut("{userId}")]
        [Authorize(Roles = "User, Administrator")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> UpdateAsync(Guid userId, UserModel user)
        {
            bool isExistName = await _usersService.IsExistUserNameAsync(user.Username);
            bool isExist = await _usersService.IsExistUserAsync(userId);

            if (!isExist || user == null || !ModelState.IsValid)
                return NotFound("User does not exist");

            if (isExistName)
                return NotFound("Username already exist");

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var currentUserId = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userMap = _mapper.Map<UserDto>(user);

            if (userMap == null)
                return NotFound("User does not exist");

            if (new Guid(currentUserId!) != userId)
                return Unauthorized("You do not have premission for this action");

            await _usersService.UpdateAsync(userId, userMap);

            return Ok(userId);
        }

        [HttpPut("name/{id}")]
        [Authorize(Roles = "User, Administrator")]
        [MapToApiVersion("2.0")]
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

        [HttpPut("email/{id}")]
        [Authorize(Roles = "User, Administrator")]
        [MapToApiVersion("2.0")]
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

        [HttpPut("password/{id}")]
        [Authorize(Roles = "User, Administrator")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> UpdatePasswordAsync(Guid id, UserPasswordModel user)
        {
            bool isExist = await _usersService.IsExistUserAsync(id);

            if (!isExist || !ModelState.IsValid)
                return NotFound("User does not exist");

            var userMap = _mapper.Map<UserDto>(user);

            if (userMap == null)
                return NotFound("User does not exist");

            await _usersService.UpdatePasswordAsync(id, userMap);

            return Ok(id);
        }
 
        [HttpPut("reset/{id}")]
        [Authorize(Roles = "Administrator")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> ResetPasswordAsync(Guid id)
        {
            bool isExist = await _usersService.IsExistUserAsync(id);

            if (!isExist || !ModelState.IsValid)
                return NotFound("User does not exist");

            var autogeneratedPassword = await _usersService.ResetPasswordAsync(id);

            return Ok(autogeneratedPassword);
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "User, Administrator")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> DeleteAsync(Guid userId)
        {
            bool isExist = await _usersService.IsExistUserAsync(userId);

            if (!isExist || !ModelState.IsValid)
                return NotFound("User does not exist");

            Task<UserDto> userToDelete = _usersService.GetByIdAsync(userId)!;

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var currentUserId = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (new Guid(currentUserId!) != userId)
                return Unauthorized("You do not have premission for this action");

            await _usersService.DeleteAsync(await userToDelete);

            return NoContent();
        }
    }
}
