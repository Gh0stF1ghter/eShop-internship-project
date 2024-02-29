using Baskets.BusinessLogic.DataTransferObjects;
using Baskets.BusinessLogic.DataTransferObjects.CreateDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.BusinessLogic.Comands.BasketItem
{
    public record CreateBasketItemComand(string UserId, CreateBasketItemDto CreateBasketItemDto) : IRequest<BasketItemDto>;
}
