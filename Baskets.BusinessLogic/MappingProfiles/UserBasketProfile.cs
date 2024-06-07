using AutoMapper;

namespace Baskets.BusinessLogic.MappingProfiles
{
    public class UserBasketProfile : Profile
    {
        public UserBasketProfile()
        {
            CreateMap<UserBasket, UserBasketDto>();
        }
    }
}