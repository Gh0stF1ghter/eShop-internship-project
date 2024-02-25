using AutoMapper;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using Catalogs.Domain.Entities.Models;

namespace Catalogs.API.MappingProfiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<ItemManipulateDto, Item>();
        }
    }
}
