using AutoMapper;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using Catalogs.Domain.Entities.Models;

namespace Catalogs.Application.MappingProfiles
{
    public class MapingProfile : Profile
    {
        public MapingProfile()
        {
            CreateMap<Item, ItemDTO>();
            CreateMap<CreateItemDTO, Item>();
        }
    }
}
