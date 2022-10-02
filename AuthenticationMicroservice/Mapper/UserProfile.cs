using AuthenticationMicroservice.Models.Request;
using AuthenticationMicroservice.Models.Response;
using AutoMapper;
using Repositories.Entities;
using Services.Dto;

namespace AuthenticationMicroservice.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserResponse>();
            CreateMap<UserResponse, UserDto>();

            CreateMap<UserModel, UserDto>();
            CreateMap<UserDto, UserModel>();

            CreateMap<UserDto, UserNameModel>();
            CreateMap<UserNameModel, UserDto>();

            CreateMap<UserEmailModel, UserDto>();
            CreateMap<UserDto, UserEmailModel>();

            CreateMap<UserPasswordModel, UserDto>();
            CreateMap<UserDto, UserPasswordModel>();

            CreateMap<UserEntity, UserDto>();
            CreateMap<UserDto, UserEntity>();
        }
    }
}
