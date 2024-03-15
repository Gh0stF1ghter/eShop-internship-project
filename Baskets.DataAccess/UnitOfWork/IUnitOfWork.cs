﻿using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.Repositories.Interfaces;

namespace Baskets.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<UserBasket> Basket { get; }
        IBasketItemRepository BasketItem { get; }
        IRepository<User> User { get; }
        IRepository<Item> Item { get; }
    }
}