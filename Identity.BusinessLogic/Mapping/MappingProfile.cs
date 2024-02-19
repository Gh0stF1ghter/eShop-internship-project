﻿using AutoMapper;
using Identity.BusinessLogic.DTOs;
using Identity.DataAccess.Models;

namespace Identity.BusinessLogic.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDTO, User>();
        }
    }
}
