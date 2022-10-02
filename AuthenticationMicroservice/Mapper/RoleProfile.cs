using AuthenticationMicroservice.Models.Request;
using AuthenticationMicroservice.Models.Response;
using AutoMapper;
using Repositories.Entities;
using Services.Dto;

namespace AuthenticationMicroservice.Mapper
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleDto, RoleResponse>();
            CreateMap<RoleResponse, RoleDto>();

            CreateMap<RoleModel, RoleDto>();
            CreateMap<RoleDto, RoleModel>();

            CreateMap<RoleEntity, RoleDto>();
            CreateMap<RoleDto, RoleEntity>();
        }
    }
}
