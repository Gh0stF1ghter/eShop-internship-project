using AutoMapper;

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