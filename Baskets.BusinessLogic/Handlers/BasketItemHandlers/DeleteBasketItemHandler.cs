using AutoMapper;
using Baskets.BusinessLogic.Comands.BasketItem;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.Handlers.BasketItemHandlers
{
    public class DeleteBasketItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<DeleteBasketItemComand>
    {
        public Task Handle(DeleteBasketItemComand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
