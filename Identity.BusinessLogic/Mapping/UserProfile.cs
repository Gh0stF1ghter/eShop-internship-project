using AutoMapper;
using Identity.BusinessLogic.DTOs;
using Identity.DataAccess.Entities.Models;

namespace Identity.BusinessLogic.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
