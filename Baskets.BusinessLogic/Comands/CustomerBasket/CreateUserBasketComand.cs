using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.BusinessLogic.Comands.CustomerBasket
{
    public record CreateUserBasketComand(string UserId) : IRequest<UserBasketDto>;
}
