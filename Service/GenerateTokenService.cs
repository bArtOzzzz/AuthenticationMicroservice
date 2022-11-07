using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Claims;
using Repositories.Abstract;
using Repositories.Entities;
using Services.Abstract;
using Services.Dto;
using System.Text;
using AutoMapper;

namespace Services
{
    public class GenerateTokenService : IGenerateTokenService
    {
        private readonly IGenerateTokenRepository _generateTokenRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GenerateTokenService(IGenerateTokenRepository generateTokenRepository,
                                    IMapper mapper,
                                    IConfiguration configuration,
                                    IRolesRepository rolesRepository)
        {
            _generateTokenRepository = generateTokenRepository;
            _mapper = mapper;
            _configuration = configuration;
            _rolesRepository = rolesRepository;
        }

        // GET
        public async Task<UserDto?> GetUserByTokenAsync(string refreshToken)
        {
            var token = await _generateTokenRepository.GetUserByTokenAsync(refreshToken);
            return _mapper.Map<UserDto>(token);
        }

        // POST
        public async Task<string?> GenerateAccessTokenAsync(UserDto user)
        {
            if (user != null)
            {
                RoleEntity? currentRole = await _rolesRepository.GetByIdAsync(user.RoleId);

                Claim[]? claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.EmailAddress!),
                    new Claim(ClaimTypes.Role, currentRole.Role!)
                };

                JwtSecurityToken? token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(5),
                    signingCredentials: new SigningCredentials(
                                        new SymmetricSecurityKey(
                                            Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
                                            SecurityAlgorithms.HmacSha512Signature));

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            return null;
        }

        public async Task<string?> CreateRefreshTokenAsync(UserDto user)
        {
            byte[]? randomNumber = new byte[32];
            using (RandomNumberGenerator? rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                string? refreshToken = Convert.ToBase64String(randomNumber);

                if (refreshToken != null)
                {
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                }

                await _generateTokenRepository.CreateRefreshTokenAsync(_mapper.Map<UserEntity>(user));

                return refreshToken;
            }
        }

        public async Task<TokenDto?> TokenAuthenticateAsync(UserDto user, string accessToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;
            ClaimsPrincipal? principal = tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateActor = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                                       Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]))
            }, out securityToken);

            JwtSecurityToken? token = new JwtSecurityToken(
                claims: principal.Claims.ToArray(),
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: new SigningCredentials(
                                    new SymmetricSecurityKey(
                                        Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
                                        SecurityAlgorithms.HmacSha512Signature));

            string? jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            TokenDto response = new TokenDto()
            {
                AccessToken = jwtToken,
                RefreshToken = await CreateRefreshTokenAsync(user)
            };

            return response;
        }
    }
}
