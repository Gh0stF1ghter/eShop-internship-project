using AutoMapper;
using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using Catalogs.Domain.Entities.Models;

namespace Catalogs.Application.MappingProfiles
{
    public class VendorProfile : Profile
    {
        public VendorProfile()
        {
            CreateMap<Vendor, VendorDto>();

            CreateMap<VendorManipulateDto, Vendor>();
        }
    }
}
