using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Entities.Models;
using AutoMapper;

namespace Catalogs.API.MappingProfiles
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandDto>();
            CreateMap<BrandManipulateDto, Brand>();
        }
    }
}
