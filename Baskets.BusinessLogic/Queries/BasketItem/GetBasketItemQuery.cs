using Baskets.BusinessLogic.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.BusinessLogic.Queries.BasketItem
{
    public record GetBasketItemQuery(string UserId, string BasketItemId) : IRequest<BasketItemDto>;
}
