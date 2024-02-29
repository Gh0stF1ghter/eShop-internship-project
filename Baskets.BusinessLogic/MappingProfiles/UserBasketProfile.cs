using AutoMapper;
using Baskets.BusinessLogic.DataTransferObjects;
using Baskets.BusinessLogic.DataTransferObjects.CreateDTOs;
using Baskets.DataAccess.Entities.Models;

namespace Baskets.BusinessLogic.MappingProfiles
{
    internal class UserBasketProfile : Profile
    {
        public UserBasketProfile()
        {
            CreateMap<UserBasket, UserBasketDto>();
        }
    }
}
