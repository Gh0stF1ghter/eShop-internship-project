using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.DataAccess.Entities.Constants.Messages
{
    public static class BasketItemMessages
    {
        public const string NotFound = "Basket item was not found";
        public const string Exists = "Basket item already exists";
        public const string NotInCurrentBasket = "Item not found in current basket";
    }
}
