using AuthenticationMicroservice.Models.Response;
using AuthenticationMicroservice.Models.Request;
using Repositories.Entities;
using Services.Dto;
using AutoMapper;

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
