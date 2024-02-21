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
            CreateMap<Item, ItemDto>();
            CreateMap<ItemManipulateDto, Item>();

            CreateMap<Brand, BrandDto>();
            CreateMap<BrandManipulateDto, Brand>();

            CreateMap<ItemType, ItemTypeDto>();
            CreateMap<ItemTypeManipulateDto, ItemType>();

            CreateMap<Vendor, VendorDto>();
            CreateMap<VendorManipulateDto, Vendor>();
        }
    }
}
