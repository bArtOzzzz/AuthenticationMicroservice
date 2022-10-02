using AuthenticationMicroservice.Models.Request;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Dto;

namespace AuthenticationMicroservice.Controllers
{
    [Route("api/")]
    [ApiController]
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
        [AllowAnonymous]
        public async Task<ActionResult> isExist(string username)
        {
            bool isExist = await _registerService.ExistsAsync(username);
            return Ok(isExist);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<ActionResult> Login(UserLoginModel userLogin)
        {
            if (userLogin == null)
                return Unauthorized("Invalid data user");

            UserDto? loggedInUser = await _authenticateService.AuthenticateAsync(userLogin.Username, userLogin.Password);

            if (loggedInUser == null)
                return NotFound("User doesn't exist");
            else if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, loggedInUser.Password))
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

        [HttpPost]
        [AllowAnonymous]
        [Route("RefreshToken")]
        public async Task<ActionResult> RefreshToken(TokenDto tokenResponse)
        {
            UserDto? currentUser = await _generateTokenService.GetUserByTokenAsync(tokenResponse.RefreshToken!);

            if (currentUser == null)
                return NotFound("User not found!");

            TokenDto? response = await _generateTokenService.TokenAuthenticateAsync(currentUser, tokenResponse.AccessToken!);

            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<ActionResult> Register(UserModel userRegister)
        {
            if (userRegister == null)
                return NotFound();

            UserDto? userMap = _mapper.Map<UserDto>(userRegister);

            if (await _registerService.ExistsAsync(userMap.Username) == true)
                return BadRequest("Username already exist.");

            if (userMap != null)
                await _registerService.RegisterAsync(userMap);
            else
                return NotFound();

            return Ok(userMap.Id);
        }
    }
}
