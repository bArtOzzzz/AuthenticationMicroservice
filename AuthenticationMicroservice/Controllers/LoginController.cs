using AuthenticationMicroservice.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Dto;
using AutoMapper;

namespace AuthenticationMicroservice.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1.0")]
    public class LoginController : Controller
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly IGenerateTokenService _generateTokenService;
        private readonly IRegisterService _registerService;
        private readonly IMapper _mapper;

        public LoginController(IAuthenticateService authenticateService,
                               IGenerateTokenService generateTokenService,
                               IRegisterService registerService,
                               IMapper mapper)
        {
            _authenticateService = authenticateService;
            _generateTokenService = generateTokenService;
            _registerService = registerService;
            _mapper = mapper;
        }

        [HttpGet("Exist/{username}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> IsExist(string username)
        {
            bool isExist = await _registerService.ExistsAsync(username);
            return Ok(isExist);
        }

        [HttpPost("Login")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> Login(UserLoginModel userLogin)
        {
            if (userLogin == null)
                return Unauthorized("Invalid data user");

            UserDto? loggedInUser = await _authenticateService.AuthenticateAsync(userLogin.Username, userLogin.Password);

            if (string.IsNullOrWhiteSpace(userLogin.Username) || string.IsNullOrWhiteSpace(userLogin.Password))
                return NotFound("User doesn't exist");
            else if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, loggedInUser!.Password))
                return Unauthorized("Username or password is incorrect");

            string? accessToken = await _generateTokenService.GenerateAccessTokenAsync(loggedInUser);
            string? refreshToken = await _generateTokenService.CreateRefreshTokenAsync(loggedInUser);

            if (accessToken == null || refreshToken == null)
                return Unauthorized("Access token or Refresh token invalid!");

            var token = new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return Ok(token);
        }

        [HttpPost("RefreshToken")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> RefreshToken(TokenDto tokenResponse)
        {
            UserDto? currentUser = await _generateTokenService.GetUserByTokenAsync(tokenResponse.RefreshToken!);

            if (currentUser == null)
                return NotFound("User not found!");

            TokenDto? response = await _generateTokenService.TokenAuthenticateAsync(currentUser, tokenResponse.AccessToken!);

            return Ok(response);
        }

        [HttpPost("Register")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> Register(UserModel userRegister)
        {
            if (userRegister == null)
                return NotFound();

            UserDto? userMap = _mapper.Map<UserDto>(userRegister);

            if (await _registerService.ExistsAsync(userMap.Username) == true)
                return Ok("Username already exist");

            if (userMap != null)
                await _registerService.RegisterAsync(userMap);
            else
                return NotFound();

            return Ok(userMap.Id);
        }
    }
}
