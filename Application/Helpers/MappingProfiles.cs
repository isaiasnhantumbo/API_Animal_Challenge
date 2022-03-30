using Application.Dtos;
using AutoMapper;
using Domain;

namespace Application.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<AppUser, UserDto>();
            CreateMap<AppUser, LoginDto>();
        }
    }
}