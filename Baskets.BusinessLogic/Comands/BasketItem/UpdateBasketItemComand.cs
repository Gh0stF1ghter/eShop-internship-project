using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.BusinessLogic.Comands.BasketItem
{
    public record UpdateBasketItemComand(string UserId, string ItemId, int Quantity) : IRequest;
}
