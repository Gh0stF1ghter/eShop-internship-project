using AutoMapper;

namespace Baskets.BusinessLogic.MappingProfiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDto>();
        }
    }
}