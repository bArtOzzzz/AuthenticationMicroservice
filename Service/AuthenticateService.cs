using Repositories.Abstract;
using Services.Abstract;
using Services.Dto;
using AutoMapper;

namespace Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IAuthenticateRepository _authenticateRepository;
        private readonly IMapper _mapper;

        public AuthenticateService(IAuthenticateRepository authenticateRepository,
                                   IMapper mapper)
        {
            _authenticateRepository = authenticateRepository;
            _mapper = mapper;
        }

        // GET
        public async Task<UserDto?> AuthenticateAsync(string username, string password)
        {
            var user = await _authenticateRepository.AuthenticateAsync(username, password);
            return _mapper.Map<UserDto>(user);
        }
    }
}
