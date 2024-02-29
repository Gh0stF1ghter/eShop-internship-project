using AutoMapper;
using Baskets.BusinessLogic.DataTransferObjects;
using Baskets.BusinessLogic.DataTransferObjects.CreateDTOs;
using Baskets.DataAccess.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
