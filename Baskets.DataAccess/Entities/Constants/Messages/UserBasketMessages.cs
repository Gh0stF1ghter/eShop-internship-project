using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.DataAccess.Entities.Constants.Messages
{
    public static class UserBasketMessages
    {
        public const string NotFound = "Basket not found or user does not exist";
        public const string Exists = "Basket already exists";
        public const string UserExists = "Unable to delete basket. User still exists";
    }
}
