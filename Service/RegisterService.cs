using Repositories.Abstract;
using Repositories.Entities;
using Services.Abstract;
using Services.Dto;
using AutoMapper;

namespace Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly IMapper _mapper;

        public RegisterService(IRegisterRepository registerRepository,
                               IMapper mapper)
        {
            _registerRepository = registerRepository;
            _mapper = mapper;
        }

        // POST
        public async Task<Guid> RegisterAsync(UserDto user)
        {
            var userMap = _mapper.Map<UserEntity>(user);
            await _registerRepository.RegisterAsync(userMap);
            user.Id = userMap.Id;
            return userMap.Id;
        }

        // EXISTS
        public async Task<bool> ExistsAsync(string username)
        {
            return await _registerRepository.ExistsAsync(username);
        }
    }
}
