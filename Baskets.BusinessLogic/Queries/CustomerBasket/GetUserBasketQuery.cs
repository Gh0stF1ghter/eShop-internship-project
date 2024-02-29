using Baskets.BusinessLogic.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.BusinessLogic.Queries.CustomerBasket
{
    public record GetUserBasketQuery(string UserId) : IRequest<UserBasketDto>;
}
