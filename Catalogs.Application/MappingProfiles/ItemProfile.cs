using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;

namespace Catalogs.Application.MappingProfiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDto>();

            CreateMap<ItemDto, ItemGrpcService.Item>();

            CreateMap<ItemManipulateDto, Item>();
        }
    }
}