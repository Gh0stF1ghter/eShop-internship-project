using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;

namespace Catalogs.Application.MappingProfiles
{
    public class ItemTypeProfile : Profile
    {
        public ItemTypeProfile()
        {
            CreateMap<ItemType, ItemTypeDto>();

            CreateMap<ItemTypeManipulateDto, ItemType>();
        }
    }
}
