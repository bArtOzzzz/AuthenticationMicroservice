using Repositories.Abstract;
using Repositories.Entities;
using Services.Abstract;
using Services.Dto;
using AutoMapper;

namespace Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public UsersService(IUsersRepository usersRepository,
                            IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        // GET
        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _usersRepository.GetAllAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(Guid userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);

            return _mapper.Map<UserDto>(user);
        }

        // PUT
        public async Task<Guid> UpdateAsync(Guid userId, UserDto user)
        {
            var userMap = _mapper.Map<UserEntity>(user);

            return await _usersRepository.UpdateAsync(userId, userMap);
        }

        public async Task<Guid> UpdateNameAsync(Guid userId, UserDto user)
        {
            var userMap = _mapper.Map<UserEntity>(user);

            return await _usersRepository.UpdateNameAsync(userId, userMap);
        }

        public async Task<Guid> UpdateEmailAsync(Guid userId, UserDto user)
        {
            var userMap = _mapper.Map<UserEntity>(user);

            return await _usersRepository.UpdateEmailAsync(userId, userMap);
        }

        public async Task<Guid> UpdatePasswordAsync(Guid userId, UserDto user)
        {
            var userMap = _mapper.Map<UserEntity>(user);

            return await _usersRepository.UpdatePasswordAsync(userId, userMap);
        }

        public async Task<string> ResetPasswordAsync(Guid userId)
        {
            return await _usersRepository.ResetPasswordAsync(userId);
        }

        // DELETE
        public async Task<bool> DeleteAsync(UserDto user)
        {
            var userMap = _mapper.Map<UserEntity>(user);
            return await _usersRepository.DeleteAsync(userMap);
        }

        // EXISTS
        public async Task<bool> IsExistUserAsync(Guid userId)
        {
            return await _usersRepository.IsExistUserAsync(userId);
        }

        public async Task<bool> IsExistUserNameAsync(string username)
        {
            return await _usersRepository.IsExistUserNameAsync(username);
        }
    }
}
