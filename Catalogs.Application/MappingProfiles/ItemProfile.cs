using AutoMapper;
using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using Catalogs.Domain.Entities.Models;

namespace Catalogs.Application.MappingProfiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDto>();

            CreateMap<Item, ItemGrpcService.Item>();

            CreateMap<ItemManipulateDto, Item>();
        }
    }
}
