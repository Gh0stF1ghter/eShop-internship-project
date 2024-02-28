using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.Repositories.Interfaces;

namespace Baskets.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<CustomerBasket> Basket { get; }
        IRepository<BasketItem> BasketItem { get; }
    }
}
