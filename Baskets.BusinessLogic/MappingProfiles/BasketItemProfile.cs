using AutoMapper;

namespace Baskets.BusinessLogic.MappingProfiles
{
    public class BasketItemProfile : Profile
    {
        public BasketItemProfile()
        {
            CreateMap<BasketItem, BasketItemDto>();

            CreateMap<CreateBasketItemDto, BasketItem>();
        }
    }
}